using System;

namespace LuaBridge.Core.StateMachine.Abstract
{
    /// <summary>
    /// The booting stage of our applications will always be nested inside another state that defines the general App flow.
    /// This is why the BootStateMachine is both a state-machine and a state 
    /// </summary>
    public interface IBootMachine : IStateMachine, IState
    {
        public event Action Finished, Failed;
    }
}