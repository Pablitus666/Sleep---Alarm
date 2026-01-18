using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Microsoft.Extensions.DependencyInjection;
using AlarmaSueño.Core;

namespace AlarmaSueño
{
    public static class WindowsIntegration
    {
        private const string TaskName = "AlarmaSueñoDailyAlarm";
        private const string AppName = "AlarmaSueño";

        public static void AddScheduledTask()
        {
            try
            {
                string? appPath = Process.GetCurrentProcess().MainModule?.FileName; // Posiblemente nulo
                if (string.IsNullOrEmpty(appPath))
                {
                    Console.WriteLine("Error: No se pudo obtener la ruta del ejecutable para la tarea programada.");
                    return;
                }
                string command = $"/create /tn \"{TaskName}\" /tr \"\"{appPath}\"\" /sc daily /st 22:00 /f";

                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe", command)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process? process = Process.Start(startInfo)) // Posiblemente nulo
                {
                    if (process == null)
                    {
                        Console.WriteLine("Error: No se pudo iniciar el proceso schtasks.exe para añadir la tarea programada.");
                        return;
                    }
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Error adding scheduled task: {error}");
                    }
                    else
                    {
                        Console.WriteLine($"Scheduled task '{TaskName}' added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
            }
        }

        public static void RemoveScheduledTask()
        {
            try
            {
                string command = $"/delete /tn \"{TaskName}\" /f";

                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe", command)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process? process = Process.Start(startInfo)) // Posiblemente nulo
                {
                    if (process == null)
                    {
                        Console.WriteLine("Error: No se pudo iniciar el proceso schtasks.exe para eliminar la tarea programada.");
                        return;
                    }
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"Error removing scheduled task: {error}");
                    }
                    else
                    {
                        Console.WriteLine($"Scheduled task '{TaskName}' removed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
            }
        }

        public static bool IsScheduledTaskExist()
        {
            try
            {
                string command = $"/query /tn \"{TaskName}\" /fo list";

                ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe", command)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process? process = Process.Start(startInfo)) // Posiblemente nulo
                {
                    if (process == null)
                    {
                        // Si no se puede iniciar el proceso, asumimos que la tarea no existe o hay un problema.
                        return false;
                    }
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    // If the task exists, schtasks /query will return 0 and output task details.
                    // If it doesn't exist, it returns 1 and an error message.
                    return process.ExitCode == 0 && !output.Contains("ERROR");
                }
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
                return false;
            }
        }

        public static void AddStartupEntry()
        {
            try
            {
                RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); // Posiblemente nulo
                if (rk == null)
                {
                    Console.WriteLine("Error: No se pudo abrir la clave de registro para el inicio.");
                    return;
                }
                string? appPath = Process.GetCurrentProcess().MainModule?.FileName; // Posiblemente nulo
                if (string.IsNullOrEmpty(appPath))
                {
                    Console.WriteLine("Error: No se pudo obtener la ruta del ejecutable para la entrada de inicio.");
                    return;
                }
                rk.SetValue(AppName, appPath);
                Console.WriteLine($"Startup entry for '{AppName}' added successfully.");
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
            }
        }

        public static void RemoveStartupEntry()
        {
            try
            {
                RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); // Posiblemente nulo
                if (rk == null)
                {
                    Console.WriteLine("Error: No se pudo abrir la clave de registro para el inicio.");
                    return;
                }
                if (rk.GetValue(AppName) != null)
                {
                    rk.DeleteValue(AppName);
                    Console.WriteLine($"Startup entry for '{AppName}' removed successfully.");
                }
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
            }
        }

        public static bool IsStartupEntryExist()
        {
            try
            {
                RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"); // Posiblemente nulo
                if (rk == null)
                {
                    return false; // Si no se puede abrir la clave, asumimos que no existe la entrada.
                }
                return rk.GetValue(AppName) != null;
            }
            catch (Exception ex)
            {
                Program.ServiceProvider?.GetService<ILogger>()?.LogException(ex);
                return false;
            }
        }
    }
}
