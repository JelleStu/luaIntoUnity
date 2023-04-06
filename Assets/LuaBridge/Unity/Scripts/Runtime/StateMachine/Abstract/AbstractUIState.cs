using Ui;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractUIState : AbstractState
    {
        protected readonly IView View;

        protected AbstractUIState(IView view)
        {
            View = view;
        }
    }
}