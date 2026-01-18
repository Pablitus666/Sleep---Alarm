using System;

namespace AlarmaSueño.Core
{
    public interface ITimer
    {
        event EventHandler? Tick;
        int Interval { get; set; }
        void Start();
        void Stop();
        bool Enabled { get; }
    }
}
