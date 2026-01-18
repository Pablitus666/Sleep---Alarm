using System;
using System.IO;
using System.Reflection;
using NAudio.Wave;
using AlarmaSueño.Core; // Changed for ILogger

namespace AlarmaSueño.Core
{
    public class AudioPlayer : IAudioPlayer
    {
        private WaveStream? audioFile;
        private WaveOutEvent? outputDevice;
        private Stream? resourceStream;
        private readonly ILogger _logger; // Added ILogger dependency

        public AudioPlayer(ILogger logger) // Constructor modified to accept ILogger
        {
            _logger = logger;
        }

        public void PlayAlarmSound(string resourceName = "alarm_sound.mp3")
        {
            // This assumes the resource is embedded in the entry assembly (UI project)
            var fullResourceName = $"AlarmaSueño.Resources.{resourceName}";
            Console.WriteLine($"Intentando reproducir recurso embebido: {fullResourceName}");

            try
            {
                // Use GetEntryAssembly to get the executable's assembly, not the Core DLL
                var assembly = Assembly.GetEntryAssembly(); 
                if (assembly == null)
                {
                    Console.WriteLine("Error: No se pudo obtener el ensamblado de entrada (Entry Assembly).");
                    return;
                }

                resourceStream = assembly.GetManifestResourceStream(fullResourceName);

                if (resourceStream == null)
                {
                    Console.WriteLine($"Error: El recurso de audio no se encontró en {fullResourceName}");
                    // Fallback for when running from a test context
                    assembly = Assembly.GetExecutingAssembly();
                    fullResourceName = $"AlarmaSueño.Core.Resources.{resourceName}";
                    resourceStream = assembly.GetManifestResourceStream(fullResourceName);
                     if (resourceStream == null)
                     {
                        Console.WriteLine($"Error: El recurso de audio tampoco se encontró en {fullResourceName}");
                        return;
                     }
                }

                // Detener y liberar recursos si ya se está reproduciendo algo
                StopAlarmSound();

                outputDevice = new WaveOutEvent();
                // Usamos Mp3FileReader que puede leer directamente de un Stream
                audioFile = new Mp3FileReader(resourceStream);
                
                outputDevice.Init(audioFile);
                outputDevice.Play();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
                Console.WriteLine("Reproducción iniciada desde recurso embebido.");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex); // Changed from Program.LogExceptionToFile
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            // Reiniciar la reproducción si la alarma debe sonar continuamente
            if (outputDevice != null && audioFile != null)
            {
                audioFile.Position = 0; // Reiniciar al principio del archivo
                outputDevice.Play();
            }
        }

        public void StopAlarmSound()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            outputDevice = null;
            
            audioFile?.Dispose();
            audioFile = null;

            resourceStream?.Dispose();
            resourceStream = null;
        }

        public void Dispose()
        {
            StopAlarmSound();
        }
    }
}
