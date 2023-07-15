using CharacterStateMachine;
using UnityEngine;

public class CharacterNopeState : IState
{
    private readonly Character character;

    public CharacterNopeState(Character character)
    {
        this.character = character;
    }
    public void Enter(IState fromState)
    {
        character.AnimatorReactor.Reacted += OnReacted;
        character.Animator.CrossFade(character.NopeAnimationName, 0.01f);
    }
    public void Exit(IState toState)
    {
        character.AnimatorReactor.Reacted -= OnReacted;
    }
    public void Update() { }

    private void OnReacted(string index)
    {
        if (index != "end nope") return;
        character.SwitchState<CharacterIdleState>();
    }
}