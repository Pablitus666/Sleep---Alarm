using System;
using System.Windows.Forms;
using AlarmaSueño.Core;

namespace AlarmaSueño.Implementations
{
    public class WinFormsTimer : AlarmaSueño.Core.ITimer
    {
        private readonly System.Windows.Forms.Timer _timer;

        public event EventHandler? Tick
        {
            add { _timer.Tick += value; }
            remove { _timer.Tick -= value; }
        }

        public int Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public bool Enabled => _timer.Enabled;

        public WinFormsTimer()
        {
            _timer = new System.Windows.Forms.Timer();
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
    }
}
