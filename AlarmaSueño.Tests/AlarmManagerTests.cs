using Xunit;
using Moq;
using System;
using AlarmaSueño.Core;

namespace AlarmaSueño.Tests
{
    public class AlarmManagerTests
    {
        [Fact]
        public void Start_StartsTimer()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);

            // Act
            alarmManager.Start();

            // Assert
            mockTimer.Verify(t => t.Start(), Times.Once);
        }

        [Fact]
        public void Stop_StopsTimer()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            alarmManager.Start(); // Start it so we can stop it

            // Act
            alarmManager.Stop();

            // Assert
            mockTimer.Verify(t => t.Stop(), Times.Once);
        }

        [Fact]
        public void Posponer_StartsTimer()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            int postponeMinutes = 5;

            // Act
            alarmManager.Posponer(postponeMinutes);

            // Assert
            // Posponer should ensure the timer is running to check for the snooze time.
            mockTimer.Verify(t => t.Start(), Times.Once);
        }

        [Fact]
        public void AlarmTimer_Tick_TriggersAlarmAfterPosponerTime()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            bool alarmTriggered = false;
            alarmManager.AlarmTriggered += (sender, e) => alarmTriggered = true;

            // Act
            // 1. Snooze for a very short time (e.g., negative minutes to simulate time has passed)
            alarmManager.Posponer(-1); 
            alarmManager.Start();
            
            // 2. Simulate the timer tick
            mockTimer.Raise(t => t.Tick += null, EventArgs.Empty);

            // Assert
            Assert.True(alarmTriggered);
            // After triggering, the timer should stop
            mockTimer.Verify(t => t.Stop(), Times.Once);
        }

        [Fact]
        public void AlarmTimer_Tick_DoesNotTriggerAlarmBeforePosponerTime()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            bool alarmTriggered = false;
            alarmManager.AlarmTriggered += (sender, e) => alarmTriggered = true;

            // Act
            // 1. Snooze for a time in the future
            alarmManager.Posponer(10); 
            alarmManager.Start();

            // 2. Simulate the timer tick
            mockTimer.Raise(t => t.Tick += null, EventArgs.Empty);

            // Assert
            Assert.False(alarmTriggered);
            // The timer should not stop if the alarm is not triggered
            mockTimer.Verify(t => t.Stop(), Times.Never);
        }
    }
}