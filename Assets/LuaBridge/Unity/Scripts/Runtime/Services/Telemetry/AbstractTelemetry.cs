using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LuaBridge.Core.Abstract;
using LuaBridge.Core.Events.Core;

namespace Services.Telemetry
{
    public abstract class AbstractTelemetry : ITelemetry
    {
        protected readonly IJsonSerializer JsonSerializer;
        protected readonly string SessionId;
        private Action<object> _replay;
        private Action<bool> _toggleFn;
        protected bool Enabled { get; private set; }

        public AbstractTelemetry(IJsonSerializer jsonSerializer)
        {
            SessionId = Guid.NewGuid().ToString();
            JsonSerializer = jsonSerializer;
        }

        public virtual void Dispose()
        {
            Enabled = false;
        }

        public abstract Task Boot();

        public void Log(object log)
        {
            if (log is ISecureEvent { IsSecured: true })
                return;
            WriteLog(new TelemetryLog()
            {
                SessionId = SessionId,
                TimeStamp = DateTime.UtcNow,
                TimeZoneId = TimeZoneInfo.Local.Id,
                RootType = log.GetType().AssemblyQualifiedName,
                Content = JsonSerializer.ToStronglyTypedJson(log)
            });
        }

        public void RegisterReplayFn(Action<object> replayFn, Action<bool> toggleFn)
        {
            _replay = replayFn;
            _toggleFn = toggleFn;
        }

        protected abstract void PurgeLogs(DateTime from);
        protected abstract void WriteLog(TelemetryLog log);
        protected abstract IEnumerable<TelemetryLog> GetLogs(string sessionId, DateTime? from, DateTime? to);
        protected abstract IEnumerable<TelemetryLog> GetLogs(DateTime? from, DateTime? to);

        public void Purge(DateTime from)
        {
            PurgeLogs(from);
        }

        public void Replay(string sessionId)
        {
            _toggleFn.Invoke(true);
            var logs = GetLogs(sessionId, null, null);
            foreach (var log in logs)
                ReplayLog(log);
            _toggleFn.Invoke(false);
        }

        public void Replay(string sessionId, DateTime? from, DateTime? to)
        {
            _toggleFn.Invoke(true);
            var logs = GetLogs(sessionId, from, to);
            foreach (var log in logs)
                ReplayLog(log);
            _toggleFn.Invoke(false);
        }

        public void Replay(DateTime? from, DateTime? to)
        {
            _toggleFn.Invoke(true);
            var logs = GetLogs(from, to);
            foreach (var log in logs)
                ReplayLog(log);
            _toggleFn.Invoke(false);
        }

        public virtual void Enable()
        {
            Enabled = true;
        }

        public virtual void Disable()
        {
            Enabled = false;
        }

        protected virtual void ReplayLog(TelemetryLog log)
        {
            var @event = JsonSerializer.FromJson(log.Content, Type.GetType(log.RootType));
            _replay.Invoke(@event);
        }
    }
}