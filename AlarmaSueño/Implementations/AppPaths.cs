using System;
using System.IO;
using AlarmaSueño.Core; // Importa la interfaz IAppPaths

namespace AlarmaSueño.Implementations
{
    public class AppPaths : IAppPaths
    {
        public string AppDataPath { get; }
        public string LogFilePath { get; }

        public AppPaths()
        {
            AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AlarmaSueño");
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }
            LogFilePath = Path.Combine(AppDataPath, "alarm_log.txt");
        }
    }
}
