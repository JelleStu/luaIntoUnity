using LuaBridge.Core.Configuration;
using UnityEngine;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractAppStateMachine : AbstractStateMachine
    {
        protected readonly AppContainer Container;
        private IBootMachine _bootMachine;

        public AbstractAppStateMachine(AppContainer container)
        {
            Container = container;
        }

        public override void Start(IState state)
        {
            if (state is IBootMachine bootMachine)
            {
                SubscribeToBootMachine(bootMachine);
            }

            base.Start(state);
        }

        private void SubscribeToBootMachine(IBootMachine bootMachine)
        {
            _bootMachine = bootMachine;
            _bootMachine.Finished += BootMachineOnFinished_Handler;
            _bootMachine.Failed += BootMachineOnFailed_Handler;
            _bootMachine.Quitted += BootMachineOnQuitted_Handler;
        }

        private void BootMachineOnQuitted_Handler()
        {
            UnSubscribeFromBootMachine();
        }

        private void UnSubscribeFromBootMachine()
        {
            if (_bootMachine == null)
                return;
            
            _bootMachine.Finished -= BootMachineOnFinished_Handler;
            _bootMachine.Failed -= BootMachineOnFailed_Handler;
            _bootMachine.Quitted -= BootMachineOnQuitted_Handler;
            _bootMachine = null;
        }

        private void BootMachineOnFailed_Handler()
        {
            Debug.LogError("Boot failed, now what? Recovery?");
            BootFailed();
        }

        private void BootMachineOnFinished_Handler()
        {
            BootFinished();
            UnSubscribeFromBootMachine();
        }

        protected abstract void BootFailed();
        protected abstract void BootFinished();

        protected override void PreQuit()
        {
            base.PreQuit();
            BootMachineOnQuitted_Handler();
        }
    }
}