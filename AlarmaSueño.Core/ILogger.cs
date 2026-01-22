using System;

namespace AlarmaSueño.Core
{
    public interface ILogger
    {
        void LogException(Exception? ex);
        void LogError(string message);
        void LogInformation(string message);
    }
}
