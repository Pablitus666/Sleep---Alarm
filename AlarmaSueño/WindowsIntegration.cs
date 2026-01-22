using AlarmaSueño.Core;
using System;
using System.Diagnostics;
using System.Reflection; 
using System.IO; // Added

namespace AlarmaSueño
{
    public static class WindowsIntegration
    {
        private const string TaskName = "AlarmaSuenoTask"; // Unique task name

        public static void CreateOrUpdateScheduledTask(DateTime alarmTime, ILogger logger)
        {
            logger.LogInformation("Using new task creation method (v3).");
            string? executablePath = Environment.ProcessPath;
            if (string.IsNullOrEmpty(executablePath))
            {
                logger.LogError("FATAL: Environment.ProcessPath es nulo o vacío. No se puede crear la tarea programada.");
                return;
            }

            string timeString = alarmTime.ToString("HH:mm");

            // Correctly format the /tr argument for schtasks. This is complex.
            // The entire value for the /tr parameter must be enclosed in quotes.
            // Inside that value, the executable path itself must also be quoted.
            // Final desired /tr value: "\"C:\Path\To\App.exe\" ALARM_TRIGGER"
            string commandToRun = $"\\\"{executablePath}\\\" ALARM_TRIGGER";

            // The arguments string passed to Process.Start needs to see the value for /tr as a single quoted string.
            string arguments = $"/create /tn \"{TaskName}\" /tr \"{commandToRun}\" /sc DAILY /st {timeString} /f /rl HIGHEST";

            ExecuteSchtasks(arguments, logger, "crear/actualizar");
        }

        public static void DeleteScheduledTask(ILogger logger)
        {
            string arguments = $"/delete /tn \"{TaskName}\" /f";
            ExecuteSchtasks(arguments, logger, "eliminar");
        }

        public static bool IsScheduledTaskActive(ILogger logger)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe")
                {
                    Arguments = $"/query /tn \"{TaskName}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                // Use Process? for nullable reference type
                using (Process? process = Process.Start(startInfo))
                {
                    if (process == null) // Explicit null check
                    {
                        logger.LogError($"Fallo al iniciar schtasks.exe para consultar la tarea. El proceso es nulo (no se pudo crear).");
                        return false;
                    }

                    process.WaitForExit();
                    // Don't read output/error unless needed, for performance/simplicity
                    // However, for debugging purposes, we want to read it.
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        logger.LogInformation($"schtasks.exe /query exited with code 0. Task '{TaskName}' found.");
                        return true;
                    }
                    else
                    {
                        logger.LogInformation($"schtasks.exe /query exited with code {process.ExitCode}. Error: {error}. Output: {output}");
                        return false; // Task not found or other error.
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Fallo al consultar schtasks.exe. Excepción: {ex.Message}");
                return false;
            }
        }

        private static void ExecuteSchtasks(string arguments, ILogger logger, string operationType)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe")
                {
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                logger.LogInformation($"Ejecutando schtasks.exe para {operationType}: {arguments}");

                // Use Process? for nullable reference type
                using (Process? process = Process.Start(startInfo))
                {
                    if (process == null) // Explicit null check
                    {
                        logger.LogError($"Fallo al iniciar schtasks.exe para {operationType}. El proceso es nulo (no se pudo crear).");
                        return; // Exit if process couldn't be started
                    }

                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        logger.LogInformation($"schtasks.exe ejecutado correctamente para {operationType}. Salida: {output}");
                    }
                    else
                    {
                        logger.LogError($"schtasks.exe finalizó con código {process.ExitCode} para {operationType}. Error: {error}. Salida: {output}");
                    }
                    // Log output and error always for better debugging, even on success.
                    logger.LogInformation($"schtasks.exe {operationType} - Salida completa: {output}");
                    if (!string.IsNullOrEmpty(error))
                    {
                        logger.LogInformation($"schtasks.exe {operationType} - Error completo: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Fallo al ejecutar schtasks.exe para {operationType}. Excepción: {ex.Message}");
            }
        }
    }
}