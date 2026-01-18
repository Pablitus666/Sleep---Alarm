using System;
using AlarmaSueño.Core;

namespace AlarmaSueño.Core
{
    public class AlarmManager : IAlarmManager
    {
        public event EventHandler? AlarmTriggered;

        private ITimer _alarmTimer; // Changed to ITimer
        private DateTime _alarmTime;
        private bool _isRunning;

        // Constructor para inyección de dependencias
        public AlarmManager(ITimer timer) // Constructor modified to accept ITimer
        {
            _alarmTimer = timer;
            _alarmTimer.Interval = 1000; // Comprobar cada segundo
            _alarmTimer.Tick += AlarmTimer_Tick;
            _isRunning = false;
        }

        public void SetAlarmTime(DateTime alarmTime)
        {
            _alarmTime = alarmTime;
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
            // Detener la alarma actual (si está sonando) y establecer una nueva
            Stop();
            _alarmTime = DateTime.Now.AddMinutes(minutes);
            Start();
        }

        private void AlarmTimer_Tick(object? sender, EventArgs e)
        {
            // Comprobar si la hora actual coincide o es posterior a la hora de la alarma
            if (DateTime.Now >= _alarmTime)
            {
                _alarmTimer.Stop(); // Detener el temporizador para evitar múltiples disparos
                _isRunning = false;
                AlarmTriggered?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
