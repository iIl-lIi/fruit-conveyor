using CharacterStateMachine;

public class CharacterIdleState : IState
{
    private readonly Character character;

    public CharacterIdleState(Character character)
    {
        this.character = character;
    }

    public void Enter(IState fromState)
    {
        InteractionBlocking.Block("grabbing");
        character.AnimatorReactor.Reacted += OnReacted;
        character.Animator.CrossFade(character.IdleTriggerName, 0.01f);
    }
    public void Exit(IState toState)
    {
        character.AnimatorReactor.Reacted -= OnReacted;
    }
    public void Update() { }

    private void OnReacted(string index)
    {
        if (index != "allow grabbing") return;
        InteractionBlocking.Unblock("grabbing");
    }
}