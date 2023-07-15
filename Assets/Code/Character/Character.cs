using UnityEngine;
using CharacterStateMachine;
using System;
using UnityEngine.Animations.Rigging;

public class Character : MonoBehaviour, IStateSwitcher
{
    public event Action<IState, IState> StateSwitched;

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public AnimatorEventsReactor AnimatorReactor { get; private set; }
    [field: SerializeField] public Basket Basket { get; private set; }
    [field: SerializeField] public TwoBoneIKConstraint IK { get; private set; }
    [field: SerializeField] public Transform HandParent { get; private set; }
    [field: SerializeField] public Transform GrabbingZonePoint { get; private set; }
    [field: SerializeField] public string IdleTriggerName { get; private set; } = "Idle";
    [field: SerializeField] public string NopeAnimationName { get; private set; } = "Nope";
    [field: SerializeField] public string GrabbingFruitAnimationName { get; private set; } = "Grabbing Fruit";
    [field: SerializeField] public string DancingTriggerName { get; private set; } = "Dancing";

    public ConveyorBeltObject GrabbedObject { get; private set; }
    public IState CurrentState { get; set; }
    public IState[] States { get; set; }

    private GrabbingTask _currentTask;
    private readonly Lazy<BlockingInfo> grabbingBlocking = 
        new (() => InteractionBlocking.GetInfo("grabbing"));

    public void SwitchState<TState>() where TState : IState
    {
        if (CurrentState is TState) return;
        foreach (var state in States)
        {
            if (state is not TState toState) continue;
            SwitchState(toState);
            break;
        }
    }
    public void SwitchState(IState toState)
    {
        if (CurrentState == toState) return;
        CurrentState?.Exit(toState);
        toState?.Enter(CurrentState);
        CurrentState = toState;
    }
    public void InitializeStates()
    {
        var idle          = new CharacterIdleState(this);
        var nope          = new CharacterNopeState(this);
        var grabbingFruit = new CharacterGrabbingFruitState(this);
        var dancing       = new CharacterDancingState(this);

        States = new IState[]
        {
            idle,
            nope,
            grabbingFruit,
            dancing
        };
    }
    public void SetTask(GrabbingTask task)
    {
        _currentTask = task;
        Basket.InitializeWithGrabbingTask(_currentTask);
    }

    private void OnStartedGrabbingFruit(ConveyorBeltObject obj)
    {
        if (_currentTask.IsCompleted
        || grabbingBlocking.Value.IsBlocked
        || GrabbedObject != null) return;

        var distance = Vector3.Distance(GrabbingZonePoint.position, obj.transform.position);
        var maxDistance = GrabbingZonePoint.localScale.x / 2;
        if (distance > maxDistance) return;

        if (obj.Id != _currentTask.taskId)
        {
            SwitchState<CharacterNopeState>();
            return;
        }

        GrabbedObject = obj;
        SwitchState<CharacterGrabbingFruitState>();
    }
    private void OnGrabbedFruit(ConveyorBeltObject obj)
    {
        if (GrabbedObject != obj) return;
        GrabbedObject = null;
        if (_currentTask.CompleteStep()) SwitchState<CharacterDancingState>();
        else SwitchState<CharacterIdleState>();
    }
    private void Awake()
    {
        CharacterEvents.StartedGrabbingFruit.Event += OnStartedGrabbingFruit;
        CharacterEvents.GrabbedFruit.Event += OnGrabbedFruit;
        InitializeStates();
        SwitchState<CharacterIdleState>();
    }
    private void Update()
    {
        CurrentState?.Update();
    }
    private void OnDestroy()
    {
        CharacterEvents.StartedGrabbingFruit.Event -= OnStartedGrabbingFruit;
        CharacterEvents.GrabbedFruit.Event -= OnGrabbedFruit;
        CurrentState?.Exit(null);
    }

#if UNITY_EDITOR
    private readonly Color _grabbingMaxDistanceColor = new Color(0, 1, 0, 0.25f);
    private void OnDrawGizmos()
    {
        if (!GrabbingZonePoint) return;
        var color = Gizmos.color;
        Gizmos.color = _grabbingMaxDistanceColor;
        var maxDistance = GrabbingZonePoint.localScale.x / 2;
        Gizmos.DrawSphere(GrabbingZonePoint.position, maxDistance);
        Gizmos.color = color;
    }
#endif
}