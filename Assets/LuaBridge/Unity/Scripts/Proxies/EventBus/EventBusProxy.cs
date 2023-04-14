using MoonSharp.Interpreter;
using UnityEngine.Scripting;

namespace LuaBridge.Proxies.EventBus
{
    public interface IEventBusProxy
    {
        void Publish(string message);
    }

    public class EventBusProxy : IEventBusProxy
    {
        private Core.Events.EventBus target;

        [MoonSharpHidden]
        public EventBusProxy(Core.Events.EventBus proxyEventBus)
        {
            this.target = proxyEventBus;
        }
        [Preserve]
        public void Publish(string message)
        {
            Core.Events.EventBus.Publish(message);
        }
    }
}