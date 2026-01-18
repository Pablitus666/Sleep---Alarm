using System.Resources;
using System.Reflection;
using System.Globalization; // Added for CultureInfo

namespace AlarmaSueño.Core
{
    public static class I18n
    {
        private static readonly ResourceManager _resourceManager = 
            new ResourceManager("AlarmaSueño.Core.Strings", Assembly.GetExecutingAssembly());

        public static string GetString(string name)
        {
            // GetString se encarga automáticamente de la cultura de la UI actual.
            return _resourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? string.Empty;
        }

        // Método de conveniencia para strings con formato.
        public static string GetString(string name, params object[] args)
        {
            var format = _resourceManager.GetString(name, CultureInfo.CurrentUICulture);
            return format == null ? string.Empty : string.Format(format, args);
        }
    }
}
