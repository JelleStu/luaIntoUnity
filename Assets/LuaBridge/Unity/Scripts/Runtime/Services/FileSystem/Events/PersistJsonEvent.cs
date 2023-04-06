using LuaBridge.Core.Events.Core;

namespace Services.Events
{
    public class PersistJsonEvent : ISecureEvent
    {
        public string Path { get; set; }
        public object Content { get; set; }
        public bool IsSecured { get; set; } = false;
    }
}