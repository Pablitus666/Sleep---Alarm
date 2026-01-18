using Xunit;
using Moq;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json; // Added
using AlarmaSueño.Core; // Reference to the Core project

namespace AlarmaSueño.Tests
{
    public class PhraseProviderTests
    {
        private readonly Mock<ILogger> _mockLogger;

        public PhraseProviderTests()
        {
            _mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public async Task LoadQuotesAsync_LoadsQuotesSuccessfullyFromFile()
        {
            // Arrange
            // PhraseProvider will load quotes.json from the test assembly
            var phraseProvider = new PhraseProvider(
                _mockLogger.Object,
                Assembly.GetExecutingAssembly(),
                "AlarmaSueño.Tests.quotes.json" // Resource name in the test project
            );

            // Act
            await phraseProvider.LoadQuotesAsync();

            // Assert
            var phrase = phraseProvider.ObtenerFrase();
            Assert.Contains(phrase, new[] { "Test Quote 1", "Test Quote 2", "Test Quote 3" });
            _mockLogger.Verify(l => l.LogException(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public async Task LoadQuotesAsync_HandlesFileNotFound_ReturnsDefaultPhraseAndLogsError()
        {
            // Arrange
            var phraseProvider = new PhraseProvider(
                _mockLogger.Object,
                Assembly.GetExecutingAssembly(),
                "NonExistent.quotes.json" // Non-existent resource name
            );

            // Act
            await phraseProvider.LoadQuotesAsync();

            // Assert
            var phrase = phraseProvider.ObtenerFrase();
            Assert.Equal("Dormir bien es invertir en ti. 🌙", phrase);
            _mockLogger.Verify(l => l.LogException(It.IsAny<FileNotFoundException>()), Times.Once);
        }

        [Fact]
        public async Task LoadQuotesAsync_HandlesCorruptJson_ReturnsDefaultPhraseAndLogsError()
        {
            // Arrange
            var corruptJsonContent = "{ \"Text\": \"Corrupt\" "; // Malformed JSON
            
            var mockAssembly = new Mock<Assembly>();
            using (var corruptStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(corruptJsonContent)))
            {
                mockAssembly.Setup(a => a.GetManifestResourceStream(It.IsAny<string>())).Returns(corruptStream);

                var phraseProvider = new PhraseProvider(
                    _mockLogger.Object,
                    mockAssembly.Object,
                    "AlarmaSueño.Tests.quotes.json" // The name doesn't matter here as stream is mocked
                );

                // Act
                await phraseProvider.LoadQuotesAsync();

                // Assert
                var phrase = phraseProvider.ObtenerFrase();
                Assert.Equal("Dormir bien es invertir en ti. 🌙", phrase);
                _mockLogger.Verify(l => l.LogException(It.IsAny<JsonException>()), Times.Once);
            }
        }

        [Fact]
        public async Task ObtenerFrase_ReturnsRandomQuote_WhenQuotesAreLoaded()
        {
            // Arrange
            var phraseProvider = new PhraseProvider(
                _mockLogger.Object,
                Assembly.GetExecutingAssembly(),
                "AlarmaSueño.Tests.quotes.json"
            );
            await phraseProvider.LoadQuotesAsync();

            // Act
            var phrase = phraseProvider.ObtenerFrase();

            // Assert
            Assert.Contains(phrase, new[] { "Test Quote 1", "Test Quote 2", "Test Quote 3" });
            
            // To be more robust, ensure there's a good distribution over many calls,
            // but for a unit test, simply checking inclusion is often sufficient.
        }

        [Fact]
        public async Task ObtenerFrase_ReturnsDefaultPhrase_WhenNoQuotesLoaded()
        {
            // Arrange
            // Create a PhraseProvider that won't load any quotes (e.g., resource not found)
            var phraseProvider = new PhraseProvider(
                _mockLogger.Object,
                Assembly.GetExecutingAssembly(),
                "NonExistent.quotes.json" // Will cause LoadQuotesAsync to fail
            );
            await phraseProvider.LoadQuotesAsync(); // Attempt to load, which will fail

            // Act
            var phrase = phraseProvider.ObtenerFrase();

            // Assert
            Assert.Equal("Dormir bien es invertir en ti. 🌙", phrase);
        }
    }
}
