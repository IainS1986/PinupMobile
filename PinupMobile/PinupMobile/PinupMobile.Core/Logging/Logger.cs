using System;

namespace PinupMobile.Core.Logging
{
    public class Logger
    {
        private static readonly Lazy<Logger> LoggerInstance = new Lazy<Logger>(() => new Logger());

        private ILogWriter _writer;

        public Logger(ILogWriter logWriter = null) 
        {
            _writer = logWriter ?? new InMemoryLogWriter(LogLevel.Diagnostic);
        }

        public static Logger Instance => LoggerInstance.Value;

        public static void Debug(string message)
        {
            Instance.LogDebug(message);
        }

        public static void Diagnostic(string message)
        {
            Instance.LogDiagnostic(message);
        }

        public static void Error(string message)
        {
            Instance.LogError(message);
        }

        public static void Fatal(string message)
        {
            Instance.LogFatal(message);
        }

        public static string GetLog()
        {
            return Instance.GetLogInternal();
        }

        public void LogError(string message)
        {
            _writer.LogLine(LogLevel.Error, message);
        }

        public void LogFatal(string message)
        {
            _writer.LogLine(LogLevel.Fatal, message);
        }

        public void LogWarning(string message)
        {
            _writer.LogLine(LogLevel.Warning, message);
        }

        public void SetLogWriter(ILogWriter writer)
        {
            _writer = writer;
        }

        public void LogDebug(string message)
        {
            _writer.LogLine(LogLevel.Debug, message);
        }

        private void LogDiagnostic(string message)
        {
            _writer.LogLine(LogLevel.Diagnostic, message);
        }

        private string GetLogInternal()
        {
            return _writer.GetLog();
        }
    }
}
