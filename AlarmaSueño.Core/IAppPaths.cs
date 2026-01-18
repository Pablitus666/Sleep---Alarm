using System;

namespace AlarmaSueño.Core
{
    public interface IAppPaths
    {
        string AppDataPath { get; }
        string LogFilePath { get; }
    }
}
