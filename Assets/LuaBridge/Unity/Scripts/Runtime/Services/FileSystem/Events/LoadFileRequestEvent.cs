using System;
using LuaBridge.Core.Events.Core;

namespace Services.Events
{
    public class LoadJsonFileRequestEvent : ISecureEvent
    {
        public string Path { get; set; }
        public Type Type { get; set; }
        public bool IsSecured { get; set; }
    }

    public class LoadJsonFileResponseEvent : ISecureEvent
    {
        public string Path { get; set; }
        public object Content { get; set; }
        public bool IsSecured { get; set; }
    }
}