using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AlarmaSueño.Core; // Changed
using AlarmaSueño.Implementations; // Changed

namespace AlarmaSueño // Changed
{
    static class Program
    {
        private const string AppMutexName = "{C9A313F8-98B2-4A7B-9B2D-2F3E7A6B2E1C}"; // Original GUID
        
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME_ALARMSUENO"); // Original Message
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, AppMutexName, out bool createdNew))
            {
                if (createdNew)
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    ApplicationConfiguration.Initialize();

                    var host = CreateHostBuilder().Build();
                    ServiceProvider = host.Services;

                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    
                    // We will migrate MainForm later
                    Application.Run(ServiceProvider.GetRequiredService<MainForm>());
                }
                else
                {
                    PostMessage(HWND_BROADCAST, WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        public static IServiceProvider? ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddSingleton<IAppPaths, AppPaths>(); // Register AppPaths
                    services.AddSingleton<ILogger, Logger>(); // Register Logger
                    services.AddSingleton<ISettingsManager, SettingsManager>();
                    services.AddSingleton<IPhraseProvider, PhraseProvider>();
                    services.AddSingleton<IAudioPlayer, AudioPlayer>();
                    services.AddSingleton<AlarmaSueño.Core.ITimer, WinFormsTimer>(); // Register ITimer with WinFormsTimer
                    services.AddSingleton<IAlarmManager, AlarmManager>(); // AlarmManager now takes ITimer
                    services.AddSingleton<MainForm>();
                });

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ServiceProvider?.GetRequiredService<ILogger>().LogException(e.Exception);
            ShowErrorMessage(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ServiceProvider?.GetRequiredService<ILogger>().LogException(e.ExceptionObject as Exception);
            ShowErrorMessage(e.ExceptionObject as Exception);
        }

        static void ShowErrorMessage(Exception? ex)
        {
            var logger = ServiceProvider?.GetService<ILogger>();
            logger?.LogException(ex); // Log the exception

            string userMessage = I18n.GetString("UnexpectedErrorClosure") +
                                  Environment.NewLine + Environment.NewLine +
                                  I18n.GetString("ErrorDetailsPrefix") + (ex?.Message ?? I18n.GetString("NoExceptionDetails"));
            
            MessageBox.Show(userMessage, I18n.GetString("AppErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
