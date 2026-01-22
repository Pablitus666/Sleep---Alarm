using System;

namespace AlarmaSueño.Core
{
    public interface IAlarmManager
    {
        event EventHandler AlarmTriggered;

        void Start();
        void Stop();
        void Posponer(int minutes);
    }
}
