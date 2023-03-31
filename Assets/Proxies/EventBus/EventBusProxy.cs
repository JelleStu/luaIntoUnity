﻿using MoonSharp.Interpreter;
using UnityEngine.Scripting;

namespace Lunacy.Proxies.EventBus
{
    public interface IEventBusProxy
    {
        void Publish(string message);
    }

    public class EventBusProxy : IEventBusProxy
    {
        private Luncay.Core.EventBus.EventBus target;

        [MoonSharpHidden]
        public EventBusProxy(Luncay.Core.EventBus.EventBus proxyEventBus)
        {
            this.target = proxyEventBus;
        }
        [Preserve]
        public void Publish(string message)
        {
            target.Publish(message);
        }
    }
}