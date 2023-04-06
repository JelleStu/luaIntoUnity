using System;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public interface IStateMachine 
    {
        public event Action<IState> StateChanged;
        public event Action Quitted;
        public IState CurrentState { get; }
        public void SetState(IState state, bool suspendCurrentState = false);
        public void Sustain<T>(bool suspendCurrentState = false) where T : IState;
        public void Quit();
    }
}