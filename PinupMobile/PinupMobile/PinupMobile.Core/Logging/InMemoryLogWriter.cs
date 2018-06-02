using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PinupMobile.Core.Logging
{
    public class InMemoryLogWriter : ILogWriter
    {
        private const int Buffer = MaxNumberLogLines;
        private const int MaxNumberLogLines = 2000;

        private readonly List<string> _logLines = new List<string>();
        private readonly object _logLock = new object();
        private readonly Action<string> _writeLogAction = line => System.Diagnostics.Debug.WriteLine(line);

        private LogLevel _level;

        public InMemoryLogWriter(LogLevel level)
        {
            _level = level;
        }

        public InMemoryLogWriter(LogLevel level, Action<string> writeLogAction) : this(level)
        {
            _writeLogAction = writeLogAction;
        }

        public string GetLog()
        {
            var sb = new StringBuilder();
            for (var i = Math.Max(0, _logLines.Count - MaxNumberLogLines); i < _logLines.Count; i++)
            {
                sb.AppendLine(_logLines[i]);
            }

            return sb.ToString();
        }

        public void LogLine(LogLevel level, string message)
        {
            if (_level > level)
            {
                return;
            }

            var threadId = Environment.CurrentManagedThreadId.ToString(CultureInfo.InvariantCulture);
            WriteLine($"{DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")} | {threadId.PadLeft(2, '0')} | {level.ToString().ToUpperInvariant()} | {message}");
        }

        private void WriteLine(string message)
        {
            lock (_logLock)
            {
                if (_logLines.Count > MaxNumberLogLines + Buffer)
                {
                    _logLines.RemoveRange(0, Buffer);
                }

                _logLines.Add(message);
            }

            _writeLogAction(message);
        }
    }
}
