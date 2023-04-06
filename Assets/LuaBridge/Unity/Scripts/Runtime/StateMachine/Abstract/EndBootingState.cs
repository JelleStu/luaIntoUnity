namespace LuaBridge.Core.StateMachine.Abstract
{
    public sealed class EndBootingState : AbstractState
    {
        public readonly bool Success;
        public override string Name => "EndBoot";

        public EndBootingState(bool success)
        {
            Success = success;
        }
    }
}