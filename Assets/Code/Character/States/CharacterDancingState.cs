using CharacterStateMachine;
using Code.UI;

public class CharacterDancingState : IState
{
    private readonly Character character;
    private bool isReacted;

    public CharacterDancingState(Character character)
    {
        this.character = character;
    }
    public void Enter(IState fromState)
    {
        character.AnimatorReactor.Reacted += OnReactedAsync;
        character.Animator.CrossFade(character.DancingTriggerName, 0.05f);
    }
    public void Exit(IState toState)
    {
        character.AnimatorReactor.Reacted -= OnReactedAsync;
        character.Animator.ResetTrigger(character.DancingTriggerName);
    }
    public void Update() { }

    private async void OnReactedAsync(string index)
    {
        if (isReacted || index != "end dancing") return;
        isReacted = true;
        var levelPasdedWindow = UIController.LoadWindow<LevelPassedUIWindow>();
        await levelPasdedWindow.Show();
    }
}