using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using LuaBridge.Core.Abstract;
using LuaBridge.Core.Events;
using LuaBridge.Core.Events.Core;
using UnityEngine;

namespace Services.Logging
{
    public class LogService : IDisposable
    {
        private readonly IJsonSerializer _serializer;
        private readonly string _path;
        private IDisposable _subscription;
        private ConcurrentQueue<LogEvent> _logQueue;
        private bool _isActive;
        private Thread _logThread;

        public LogService(IJsonSerializer serializer, string path)
        {
            _serializer = serializer;
            _path = path;
            _logQueue = new ConcurrentQueue<LogEvent>();
            _subscription = EventBus.Subscribe<LogEvent>(Log);
            _logThread = new Thread(WriteToLog) { IsBackground = true };
            _isActive = true;
            _logThread.Start();
        }

        private void Log(LogEvent log)
        {
            if (_isActive)
                _logQueue.Enqueue(log);
        }

        public void Dispose()
        {
            _isActive = false;
            _logThread.Abort();
            _logThread = null;
            _logQueue.Clear();
            _subscription?.Dispose();
        }

        private void WriteToLog()
        {
            var logFile = new FileInfo(_path);
            if (logFile.Exists)
            {
                var file = logFile.FullName.Replace(logFile.Extension, "");
                var previousName = $"{file}_Previous{logFile.Extension}";
                if (File.Exists(previousName))
                    File.Delete(previousName);
                logFile.MoveTo(previousName);
            }
            else if (!Directory.Exists(Path.GetDirectoryName(_path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path));
            }

            using var sr = new StreamWriter(_path, true);
            sr.AutoFlush = true;
            while (_isActive)
            {
                if (_logQueue.IsEmpty)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (_logQueue.TryDequeue(out var log))
                {
                    string entry = log.Content == null
                        ? $"{log.LocalTime:yyyy-MM-ddTHH:mm:ss} [{log.Type}]: {log.Message}"
                        : $"{log.LocalTime:yyyy-MM-ddTHH:mm:ss} [{log.Type}]: Message = [{log.Message}] Content = [{_serializer.ToJson(log.Content)}]";

                    Debug.Log(entry);
                    sr.WriteLine(entry);
                }
            }
        }
    }
}