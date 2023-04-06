using System;
using LuaBridge.Core.Configuration;
using UnityEngine;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractBootStateMachine : AbstractStateMachine, IBootMachine
    {
        public string Name => "Boot";
        protected readonly AppContainer Container;
        private IStateMachine _stateMachine;
        public event Action Finished, Failed;


        public AbstractBootStateMachine(AppContainer container)
        {
            Container = container;
            StateChanged += OnStateChanged;
        }

        public void OnSuspend()
        {
            Debug.LogError("Boot StateMachine cannot be suspended!");
            throw new System.NotImplementedException();
        }

        public void OnSustain()
        {
            Debug.LogError("Boot StateMachine cannot be sustained!");
            throw new System.NotImplementedException();
        }

        private void Finish()
        {
            Finished?.Invoke();
            Quit();
        }

        private void Fail()
        {
            Failed?.Invoke();
            Quit();
        }

        public void OnEnter(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            Enter();
        }

        public void OnExit()
        {
            StateChanged -= OnStateChanged;
            Exit();
        }

        protected abstract void Enter();

        protected abstract void Exit();

        private void OnStateChanged(IState state)
        {
            if (state is not EndBootingState ebs)
                return;

            if (ebs.Success)
                Finish();
            else
                Fail();
        }

        public void Dispose()
        {
        }
    }
}