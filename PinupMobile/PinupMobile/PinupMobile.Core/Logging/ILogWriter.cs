namespace PinupMobile.Core.Logging
{
    public interface ILogWriter
    {
        void LogLine(LogLevel level, string message);

        string GetLog();
    }
}
