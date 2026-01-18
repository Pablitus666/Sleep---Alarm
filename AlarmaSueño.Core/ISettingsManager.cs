using System.Threading.Tasks;

namespace AlarmaSueño.Core
{
    public interface ISettingsManager
    {
        Task<AppSettings> LoadSettingsAsync();
        Task SaveSettingsAsync(AppSettings settingsToSave);
    }
}
