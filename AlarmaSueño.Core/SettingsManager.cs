using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AlarmaSueño.Core; // Necesario para IAppPaths y ILogger

namespace AlarmaSueño.Core
{
    public class SettingsManager : ISettingsManager
    {
        private readonly string _settingsFilePath;
        private readonly ILogger _logger; // Added ILogger dependency

        public SettingsManager(IAppPaths appPaths, ILogger logger) // Constructor modified to accept ILogger
        {
            _settingsFilePath = Path.Combine(appPaths.AppDataPath, "settings.json");
            _logger = logger;
        }

        public async Task<AppSettings> LoadSettingsAsync()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = await File.ReadAllTextAsync(_settingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        // Asegurarse de que los valores sean válidos después de la deserialización
                        if (settings.SnoozeMinutes <= 0) settings.SnoozeMinutes = 5;
                        if (settings.PostponeMinutes <= 0) settings.PostponeMinutes = 10;
                        return settings;
                    }
                }
            }
            catch (Exception ex)
            {
                // El Console.WriteLine es útil para depuración en tiempo real si el log no es accesible.
                Console.WriteLine($"Error al cargar la configuración: {ex.Message}");
                _logger.LogException(ex); // Changed from Program.LogExceptionToFile
            }
            // Retornar una nueva instancia de AppSettings con los valores por defecto del constructor
            return new AppSettings();
        }

        public async Task SaveSettingsAsync(AppSettings settingsToSave)
        {
            try
            {
                string json = JsonSerializer.Serialize(settingsToSave, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                // El Console.WriteLine es útil para depuración en tiempo real si el log no es accesible.
                Console.WriteLine($"Error al guardar la configuración: {ex.Message}");
                _logger.LogException(ex); // Changed from Program.LogExceptionToFile
            }
        }
    }
}
