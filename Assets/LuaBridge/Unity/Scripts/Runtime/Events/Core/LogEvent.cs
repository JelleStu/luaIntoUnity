using System;

namespace LuaBridge.Core.Events.Core
{
    public class LogEvent : ISecureEvent
    {
        public enum LogType
        {
            Log = 0,
            Warning = 1,
            Error = 2,
            Exception = 3
        }

        public LogType Type { get; private set; }
        public string Message { get; private set; }
        public object Content { get; private set; }
        public DateTime UtcTime { get; private set; }
        public DateTime LocalTime { get; private set; }
        public string TimeZoneId { get; private set; }
        public bool IsSecured { get; set; } = true;


        private LogEvent(LogType type, string message, object content = null)
        {
            UtcTime = DateTime.UtcNow;
            LocalTime = DateTime.Now;
            TimeZoneId = TimeZoneInfo.Local.Id;
            Type = type;
            Message = message;
            Content = content;
        }

        public static void AppendLog(string message, object content = null)
        {
            EventBus.Publish(new LogEvent(LogType.Log, message, content));
        }

        public static void AppendWarning(string message, object content = null)
        {
            EventBus.Publish(new LogEvent(LogType.Warning, message, content));
        }

        public static void AppendError(string message, object content = null)
        {
            EventBus.Publish(new LogEvent(LogType.Error, message, content));
        }

        public static void AppendException(string message, Exception e, object content = null)
        {
            EventBus.Publish(new LogEvent(LogType.Exception, $"{message} {e} | {e.Message}", content));
        }
    }
}