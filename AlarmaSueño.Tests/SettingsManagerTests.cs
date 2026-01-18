using Xunit;
using Moq;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AlarmaSueño.Core;

namespace AlarmaSueño.Tests
{
    public class SettingsManagerTests : IDisposable
    {
        private readonly Mock<IAppPaths> _mockAppPaths;
        private readonly Mock<ILogger> _mockLogger;
        private readonly string _testAppDataPath;
        private readonly string _testSettingsFilePath;
        private readonly SettingsManager _settingsManager;

        public SettingsManagerTests()
        {
            _mockAppPaths = new Mock<IAppPaths>();
            _mockLogger = new Mock<ILogger>();
            
            // Create a unique temporary directory for each test run to avoid conflicts
            _testAppDataPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testAppDataPath); // Ensure the directory exists
            
            _testSettingsFilePath = Path.Combine(_testAppDataPath, "settings.json");

            // Setup the mock to return this unique temporary path
            _mockAppPaths.Setup(ap => ap.AppDataPath).Returns(_testAppDataPath);
            _mockAppPaths.Setup(ap => ap.LogFilePath).Returns(Path.Combine(_testAppDataPath, "test_log.txt")); // Dummy log path for unique test runs

            _settingsManager = new SettingsManager(_mockAppPaths.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task LoadSettingsAsync_ReturnsDefaultSettings_WhenFileDoesNotExist()
        {
            // Arrange
            // File does not exist by default due to unique path per test

            // Act
            var settings = await _settingsManager.LoadSettingsAsync();

            // Assert
            Assert.NotNull(settings);
            // Assert on hour only, as DateTime.Today is dynamic and can cause issues with full date comparison
            Assert.Equal(7, settings.AlarmTime.Hour); 
            Assert.Equal(5, settings.SnoozeMinutes);
            Assert.Equal(10, settings.PostponeMinutes);
            Assert.False(settings.AutoStart);
            Assert.False(settings.IsLocked);
        }

        [Fact]
        public async Task LoadSettingsAsync_LoadsSettingsFromFile_WhenFileExists()
        {
            // Arrange
            var expectedSettings = new AppSettings
            {
                // Use DateTime.SpecifyKind to ensure consistency, matching AppSettings constructor's Unspecified kind
                AlarmTime = DateTime.Today.AddHours(8), 
                SnoozeMinutes = 7,
                PostponeMinutes = 15,
                AutoStart = true,
                IsLocked = true
            };
            string json = JsonSerializer.Serialize(expectedSettings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_testSettingsFilePath, json);

            // Act
            var actualSettings = await _settingsManager.LoadSettingsAsync();

            // Assert
            Assert.NotNull(actualSettings);
            Assert.Equal(expectedSettings.AlarmTime, actualSettings.AlarmTime); // Direct comparison for Unspecified kind
            Assert.Equal(expectedSettings.SnoozeMinutes, actualSettings.SnoozeMinutes);
            Assert.Equal(expectedSettings.PostponeMinutes, actualSettings.PostponeMinutes);
            Assert.Equal(expectedSettings.AutoStart, actualSettings.AutoStart);
            Assert.Equal(expectedSettings.IsLocked, actualSettings.IsLocked);
        }

        [Fact]
        public async Task LoadSettingsAsync_HandlesCorruptFile_ReturnsDefaultSettingsAndLogsError()
        {
            // Arrange
            await File.WriteAllTextAsync(_testSettingsFilePath, "{ malformed json "); // Corrupt JSON

            // Act
            var settings = await _settingsManager.LoadSettingsAsync();

            // Assert
            Assert.NotNull(settings);
            Assert.Equal(7, settings.AlarmTime.Hour); // Check default values
            _mockLogger.Verify(l => l.LogException(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task SaveSettingsAsync_SavesSettingsToFile()
        {
            // Arrange
            var settingsToSave = new AppSettings
            {
                AlarmTime = DateTime.Today.AddHours(9), // Use DateTime.Today.AddHours for consistency
                SnoozeMinutes = 10,
                PostponeMinutes = 20,
                AutoStart = false,
                IsLocked = false
            };

            // Act
            await _settingsManager.SaveSettingsAsync(settingsToSave);

            // Assert
            Assert.True(File.Exists(_testSettingsFilePath));
            string json = await File.ReadAllTextAsync(_testSettingsFilePath);
            var actualSettings = JsonSerializer.Deserialize<AppSettings>(json);

            Assert.NotNull(actualSettings);
            Assert.Equal(settingsToSave.AlarmTime, actualSettings.AlarmTime); // Direct comparison for Unspecified kind
            Assert.Equal(settingsToSave.SnoozeMinutes, actualSettings.SnoozeMinutes);
            Assert.Equal(settingsToSave.PostponeMinutes, actualSettings.PostponeMinutes);
            Assert.Equal(settingsToSave.AutoStart, actualSettings.AutoStart);
            Assert.Equal(settingsToSave.IsLocked, actualSettings.IsLocked);
        }

        [Fact]
        public async Task SaveSettingsAsync_LogsError_WhenSaveFailsDueToException()
        {
            // Arrange
            // Simulate AppDataPath returning an invalid path for Save operation
            var failingPathMockAppPaths = new Mock<IAppPaths>();
            
            // This setup must return a path where File.WriteAllTextAsync will fail.
            failingPathMockAppPaths.Setup(ap => ap.AppDataPath).Returns("C:\\_invalid_nonexistent_path_\\"); // Simulate path that will cause error
            failingPathMockAppPaths.Setup(ap => ap.LogFilePath).Returns(Path.Combine(_testAppDataPath, "test_log.txt")); // Valid log path

            var failingSettingsManager = new SettingsManager(failingPathMockAppPaths.Object, _mockLogger.Object);
            var settingsToSave = new AppSettings();

            // Act
            await failingSettingsManager.SaveSettingsAsync(settingsToSave);

            // Assert
            _mockLogger.Verify(l => l.LogException(It.IsAny<Exception>()), Times.Once);
            // Ensure no settings file was created at the test's original path
            Assert.False(File.Exists(_testSettingsFilePath)); 
        }

        // Cleanup after each test by xunit's IDisposable implementation
        public void Dispose()
        {
            if (Directory.Exists(_testAppDataPath))
            {
                Directory.Delete(_testAppDataPath, true); // Delete the unique temporary directory and its contents
            }
        }
    }
}
