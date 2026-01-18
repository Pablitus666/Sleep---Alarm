using System;

namespace AlarmaSueño.Core
{
    public class AppSettings
    {
        public DateTime AlarmTime { get; set; }
        public int SnoozeMinutes { get; set; } = 5;
        public int PostponeMinutes { get; set; } = 10;
        public bool AutoStart { get; set; }
        public bool IsLocked { get; set; }

        public AppSettings()
        {
            AlarmTime = DateTime.Today.AddHours(7); // Default to 7 AM
        }
    }
}
