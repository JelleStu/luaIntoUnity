using System;

namespace Services.Telemetry
{
    public struct TelemetryLog
    {
        public string SessionId { get; set; }
        public string RootType { get; set; }
        public DateTime TimeStamp;
        public string TimeZoneId;
        public string Content;
    }
}