using System;
using AlarmaSueño.Core;

namespace AlarmaSueño.Core
{
    public class AlarmManager : IAlarmManager
    {
        public event EventHandler? AlarmTriggered;

        private readonly ITimer _alarmTimer;
        // _nextTriggerTime now represents the next snooze trigger time
        private DateTime _nextTriggerTime;  
        private bool _isRunning;

        public AlarmManager(ITimer timer)
        {
            _alarmTimer = timer;
            _alarmTimer.Interval = 1000; // Comprobar cada segundo
            _alarmTimer.Tick += AlarmTimer_Tick;
            _isRunning = false;
            _nextTriggerTime = DateTime.MaxValue; // Initialize to prevent immediate trigger
        }



        public void Start()
        {
            if (!_isRunning)
            {
                _alarmTimer.Start();
                _isRunning = true;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _alarmTimer.Stop();
                _isRunning = false;
            }
        }

        public void Posponer(int minutes)
        {
            // Posponer establece la próxima activación en el futuro cercano, sin afectar la configuración principal.
            _nextTriggerTime = DateTime.Now.AddMinutes(minutes);
            Start(); // Asegurarse de que el temporizador esté corriendo
        }

        // Reset is no longer needed as daily scheduling is external.
        // public void Reset() { }

        private void AlarmTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isRunning) return;

            // This logic is now only for snooze functionality.
            if (DateTime.Now >= _nextTriggerTime)
            {
                // Stop temporarily to avoid multiple triggers while processing the event.
                this.Stop();

                AlarmTriggered?.Invoke(this, EventArgs.Empty);

                // No recalculation for next day here. That's external.
                // The snooze will be re-triggered by the user dismissing/postponing again.
            }
        }
    }
}
