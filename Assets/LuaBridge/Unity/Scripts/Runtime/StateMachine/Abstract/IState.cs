using System;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public interface IState : IDisposable
    {
        public string Name { get; }
        public void OnSuspend();
        public void OnSustain();
        public void OnEnter(IStateMachine stateMachine);
        public void OnExit();
    }
}