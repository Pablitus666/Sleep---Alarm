using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AlarmaSueño.Core; // Changed for ILogger

namespace AlarmaSueño.Core
{
    public class PhraseProvider : IPhraseProvider
    {
        private List<Quote> quotes = new List<Quote>();
        private Random random;
        private readonly ILogger _logger; // Added ILogger dependency
        private readonly Assembly _resourceAssembly;
        private readonly string _quotesResourceName;


        public PhraseProvider(ILogger logger, Assembly? resourceAssembly = null, string? quotesResourceName = null)
        {
            random = new Random();
            _logger = logger;
            _resourceAssembly = resourceAssembly ?? Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly(); // Fallback
            _quotesResourceName = quotesResourceName ?? "AlarmaSueño.quotes.json";
        }

        public async Task LoadQuotesAsync()
        {
            try
            {
                using (Stream? stream = _resourceAssembly.GetManifestResourceStream(_quotesResourceName))
                {
                    if (stream == null)
                    {
                        _logger.LogError($"PhraseProvider Error: Resource stream was null for '{_quotesResourceName}' in assembly '{_resourceAssembly.FullName}'.");
                        return;
                    }
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonString = await reader.ReadToEndAsync();
                        var loadedQuotes = JsonSerializer.Deserialize<List<Quote>>(jsonString);
                        if (loadedQuotes != null)
                        {
                            quotes = loadedQuotes;
                            _logger.LogInformation($"Successfully loaded {quotes.Count} quotes.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        public string ObtenerFrase()
        {
            if (quotes.Count == 0)
            {
                return "Dormir bien es invertir en ti. 🌙";
            }

            int index = random.Next(quotes.Count);
            return quotes[index].Text; // Devolver solo el texto de la cita
        }
    }
    public class Quote
    {
        public required string Text { get; set; }
    }
}