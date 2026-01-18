using System;

namespace AlarmaSueño.Core
{
    public interface ILogger
    {
        void LogException(Exception? ex);
    }
}
