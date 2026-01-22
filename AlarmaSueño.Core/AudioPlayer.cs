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
        private readonly ILogger _logger;
        private readonly Assembly _resourceAssembly;

        public AudioPlayer(ILogger logger, Assembly resourceAssembly)
        {
            _logger = logger;
            _resourceAssembly = resourceAssembly;
        }

        public void PlayAlarmSound(string resourceName = "alarm_sound.mp3")
        {
            try
            {
                // --- ROBUST RESOURCE FINDING ---
                var allResourceNames = _resourceAssembly.GetManifestResourceNames();
                string? actualResourceName = allResourceNames.FirstOrDefault(name => name.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(actualResourceName))
                {
                    _logger.LogError($"AudioPlayer Error: Could not find any resource ending with '{resourceName}' in assembly '{_resourceAssembly.FullName}'.");
                    return;
                }
                // --- END ROBUST RESOURCE FINDING ---

                resourceStream = _resourceAssembly.GetManifestResourceStream(actualResourceName);

                if (resourceStream == null)
                {
                    _logger.LogError($"AudioPlayer Error: Resource stream was unexpectedly null for '{actualResourceName}'.");
                    return;
                }

                // Copy to MemoryStream to ensure it's seekable and can be read multiple times.
                MemoryStream memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);
                memoryStream.Position = 0; // Reset position to the beginning

                // Detener y liberar recursos si ya se está reproduciendo algo
                StopAlarmSound();

                outputDevice = new WaveOutEvent();
                audioFile = new Mp3FileReader(memoryStream); // Use the MemoryStream
                
                outputDevice.Init(audioFile);
                outputDevice.Play();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
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
