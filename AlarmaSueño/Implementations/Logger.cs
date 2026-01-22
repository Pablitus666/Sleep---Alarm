using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection; // For GetRequiredService
using AlarmaSueño.Core;

namespace AlarmaSueño.Implementations
{
    public class Logger : ILogger
    {
        private readonly IAppPaths _appPaths;

        public Logger(IAppPaths appPaths)
        {
            _appPaths = appPaths;
        }

        public void LogException(Exception? ex)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]" + Environment.NewLine +
                                $"Details: {(ex?.Message ?? "No exception details were provided.")}" + Environment.NewLine +
                                $"Stack Trace: {ex?.StackTrace}" + Environment.NewLine +
                                "---------------------------------------------------" + Environment.NewLine;
            
            try
            {
                File.AppendAllText(_appPaths.LogFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Failed to write to log: {logEx.Message}");
            }
        }

        public void LogError(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}" + Environment.NewLine +
                                "---------------------------------------------------" + Environment.NewLine;
            try
            {
                File.AppendAllText(_appPaths.LogFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Failed to write to log: {logEx.Message}");
            }
        }

        public void LogInformation(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}" + Environment.NewLine +
                                "---------------------------------------------------" + Environment.NewLine;
            try
            {
                File.AppendAllText(_appPaths.LogFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Failed to write to log: {logEx.Message}");
            }
        }
    }
}
