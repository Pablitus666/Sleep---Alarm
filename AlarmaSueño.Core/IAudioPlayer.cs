using System;

namespace AlarmaSueño.Core
{
    public interface IAudioPlayer : IDisposable
    {
        void PlayAlarmSound(string resourceName = "alarm_sound.mp3");
        void StopAlarmSound();
    }
}
