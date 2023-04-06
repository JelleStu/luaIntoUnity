using System;
using LuaBridge.Core.Services.Abstract;

namespace Services.Telemetry
{
    public interface ITelemetry : IAsyncBootService
    {
        public void Log(object log);
        public void RegisterReplayFn(Action<object> replayFn, Action<bool> toggleFn);
        public void Purge(DateTime from);
        public void Replay(string sessionId);
        public void Replay(string sessionId, DateTime? from, DateTime? to);
        public void Replay(DateTime? from, DateTime? to);
        void Enable();
        void Disable();
    }
}