namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractBootState : AbstractState
    {
        protected void Finish()
        {
            ChangeState(new EndBootingState(true));
        }

        protected void Fail()
        {
            ChangeState(new EndBootingState(false));
        }
    }
}