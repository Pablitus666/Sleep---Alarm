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
            // I18n.GetString might not be initialized yet if an error occurs very early.
            // Use a fallback for I18n.GetString if ServiceProvider is null.
            string errorDetailsPrefix = Program.ServiceProvider != null ? I18n.GetString("ErrorDetailsPrefix") : "Details: ";
            string noExceptionDetails = Program.ServiceProvider != null ? I18n.GetString("NoExceptionDetails") : "No exception details were provided.";
            string logWriteFailure = Program.ServiceProvider != null ? I18n.GetString("LogWriteFailure") : "Failed to write to log: ";


            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]" + Environment.NewLine +
                                $"{errorDetailsPrefix}{(ex?.Message ?? noExceptionDetails)}" + Environment.NewLine +
                                $"Stack Trace: {ex?.StackTrace}" + Environment.NewLine +
                                "---------------------------------------------------" + Environment.NewLine;
            
            try
            {
                File.AppendAllText(_appPaths.LogFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"{logWriteFailure}{logEx.Message}");
            }
        }
    }
}
