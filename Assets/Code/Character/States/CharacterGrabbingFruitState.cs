using CharacterStateMachine;
using UnityEngine;

public class CharacterGrabbingFruitState : IState
{
    private readonly Character character;
    private bool _touched;

    public CharacterGrabbingFruitState(Character character)
    {
        this.character = character;
    }
    public void Enter(IState fromState)
    {
        character.AnimatorReactor.Reacted += OnReacted;
        var transitionDuration = fromState is CharacterIdleState ? 0.015f : 0.075f;
        character.Animator.CrossFade(character.GrabbingFruitAnimationName, transitionDuration);
    }
    public void Exit(IState toState)
    {
        character.AnimatorReactor.Reacted -= OnReacted;
        character.IK.weight = 0;
    }
    public void Update() => UpdateIK();

    private void UpdateIK()
    {
        var ik = character.IK;
        if (_touched)
        {
            ik.weight = 0;
            return;
        }

        var target = ik.data.target;
        ik.weight = character.Animator.GetFloat("IKWeight");
        target.position = character.GrabbedObject.transform.position;
    }
    private void TouchIK()
    {
        var objTransform = character.GrabbedObject.transform;
        objTransform.parent = character.HandParent;
        objTransform.localPosition = Vector3.zero;
        objTransform.localRotation = Quaternion.identity;
        _touched = true;
    }
    private void OnReacted(string index)
    {
        if (index == "touch")
        {
            CharacterEvents.TouchedBeforeGrabbedFruit.SafeInvoke(character.GrabbedObject);
            TouchIK();
        }
        else if (index == "put")
        {
            character.Basket.PutConveyorBeltObject(character.GrabbedObject);
        }
        else if (index == "end grabbing")
        {
            CharacterEvents.GrabbedFruit.SafeInvoke(character.GrabbedObject);
            _touched = false;
        }
    }
}