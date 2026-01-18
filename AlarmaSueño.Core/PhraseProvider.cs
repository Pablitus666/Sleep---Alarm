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
                // This assumes the resource is embedded in the entry assembly (UI project)
                // var assembly = Assembly.GetEntryAssembly(); // No longer directly used
                // if (assembly == null)
                // {
                //     throw new InvalidOperationException("No se pudo obtener el ensamblado de entrada (Entry Assembly).");
                // }
                
                // El nombre del recurso es "NamespaceDelProyectoUI.NombreDelArchivo"
                // var resourceName = "AlarmApp.UI.quotes.json"; // No longer hardcoded

                using (Stream? stream = _resourceAssembly.GetManifestResourceStream(_quotesResourceName))
                {
                    if (stream == null)
                    {
                        throw new FileNotFoundException($"El recurso '{_quotesResourceName}' no se encontró en el ensamblado '{_resourceAssembly.FullName}'.", _quotesResourceName);
                    }
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonString = await reader.ReadToEndAsync();
                        var loadedQuotes = JsonSerializer.Deserialize<List<Quote>>(jsonString);
                        if (loadedQuotes != null)
                        {
                            quotes = loadedQuotes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                _logger.LogException(ex); // Changed from Program.LogExceptionToFile
                // The quotes list will remain empty, and ObtenerFrase will return a default string.
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