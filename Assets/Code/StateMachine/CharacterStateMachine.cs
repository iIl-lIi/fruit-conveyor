using System;

namespace CharacterStateMachine
{
    public interface IState
    {
        void Enter(IState fromState);
        void Exit(IState toState);
        void Update();
    }

    public interface IStateSwitcher
    {
        event Action<IState, IState> StateSwitched;
        IState CurrentState { get; set; }
        IState[] States { get; set; }
        void InitializeStates();
        void SwitchState<TState>() where TState : IState;
        void SwitchState(IState stateType);
    }
}