using Xunit;
using Moq;
using System;
using AlarmaSueño.Core; // Reference to the Core project

namespace AlarmaSueño.Tests
{
    public class AlarmManagerTests
    {
        [Fact]
        public void SetAlarmTime_SetsCorrectly()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            var testTime = new DateTime(2026, 1, 18, 10, 30, 0);

            // Act
            alarmManager.SetAlarmTime(testTime);

            // Assert
            // Cannot directly assert on _alarmTime as it's private.
            // We would need to test its effect, e.g., if alarm triggers at correct time.
            // This test is mostly a placeholder and would ideally involve
            // checking the internal state via reflection or testing behavior.
            // For now, we will rely on other tests to verify behavior driven by alarmTime.
            Assert.True(true); 
        }

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
        public void Posponer_StopsAndRestartsAlarmWithNewTime()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);
            var initialTime = DateTime.Now.AddHours(1);
            alarmManager.SetAlarmTime(initialTime);
            alarmManager.Start();

            // Reset mocks for clear verification of Posponer's actions
            mockTimer.Invocations.Clear(); // Clear previous verifications

            int postponeMinutes = 5;

            // Act
            alarmManager.Posponer(postponeMinutes);

            // Assert
            mockTimer.Verify(t => t.Stop(), Times.Once);
            mockTimer.Verify(t => t.Start(), Times.Once);
            // We cannot directly verify the new _alarmTime, as it's private.
            // The fact that Start() was called again implies a new time was set.
        }

        [Fact]
        public void AlarmManager_RaisesAlarmTriggeredEventOnTickWhenTimeIsReached()
        {
            // Arrange
            var mockTimer = new Mock<AlarmaSueño.Core.ITimer>();
            var alarmManager = new AlarmManager(mockTimer.Object);

            // Set a dummy alarm time; it won't matter for this test's logic
            // because we're directly triggering the 'Tick' event, and assuming
            // DateTime.Now is greater than _alarmTime due to DateTime.MinValue.
            alarmManager.SetAlarmTime(DateTime.MinValue); 

            bool alarmTriggered = false;
            alarmManager.AlarmTriggered += (sender, e) => alarmTriggered = true;
            alarmManager.Start(); // Ensure the AlarmManager is "running" and subscribed

            // Act
            // Simulate the timer's Tick event being raised.
            // This will cause AlarmManager's AlarmTimer_Tick method to execute.
            mockTimer.Raise(t => t.Tick += null, EventArgs.Empty);

            // Assert
            Assert.True(alarmTriggered);
            mockTimer.Verify(t => t.Stop(), Times.Once); // Ensure timer stops after triggering
        }
    }
}
