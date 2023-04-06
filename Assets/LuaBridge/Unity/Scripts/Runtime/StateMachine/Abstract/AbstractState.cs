using UnityEngine;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractState : IState
    {
        public abstract string Name { get; }
        private IStateMachine _stateMachine;
        public bool IsSuspended { get; private set; }

        protected AbstractState()
        {
        }

        public void OnSuspend()
        {
            IsSuspended = true;
            Suspend();
        }

        public void OnSustain()
        {
            IsSuspended = false;
            Sustain();
        }

        public void OnEnter(IStateMachine stateMachine)
        {
            IsSuspended = false;
            _stateMachine = stateMachine;
            Debug.Log($"{GetType().Name} Entered");
            Enter();
        }

        public void OnExit()
        {
            Exit();
            Dispose();
            Debug.Log($"{GetType().Name} Exited");
        }

        protected virtual void Suspend()
        {
        }

        protected virtual void Sustain()
        {
        }

        protected virtual void Enter()
        {
        }

        protected virtual void Exit()
        {
        }
        protected void ChangeState(IState state, bool suspendCurrentState = false)
        {
            _stateMachine.SetState(state, suspendCurrentState);
        }

        protected void Sustain<T>(bool suspendCurrentState = false) where T : IState
        {
            _stateMachine.Sustain<T>(suspendCurrentState);
        }

        public virtual void Dispose()
        {
        }
    }
}