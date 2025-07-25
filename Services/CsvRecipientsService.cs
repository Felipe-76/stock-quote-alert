using StockQuoteAlert.DataModels;
using StockQuoteAlert.Services;

namespace StockQuoteAlert.Services
{
    /// <summary>
    /// CSV-based implementation of email recipients service.
    /// </summary>
    public class CsvRecipientsService : IRecipientsService
    {
        private readonly string _csvFilePath;
        private readonly string _separator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRecipientsService"/> class.
        /// </summary>
        /// <param name="csvFilePath">Path to the CSV file containing email recipients.</param>
        /// <param name="separator">CSV columns separator.</param>
        public CsvRecipientsService(string csvFilePath = "email_recipients.csv", string separator = ",")
        {
            _csvFilePath = csvFilePath;
            _separator = separator;
        }

        /// <summary>
        /// Gets the list of email recipients from the CSV file.
        /// </summary>
        /// <returns>List of email recipients.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the CSV file is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there's an error reading the file or no valid recipients are found.</exception>
        public List<string> GetRecipients()
        {
            var recipients = new List<string>();

            if (!File.Exists(_csvFilePath))
            {
                throw new FileNotFoundException($"Email recipients file not found: {_csvFilePath}");
            }

            try
            {
                var lines = File.ReadAllLines(_csvFilePath);
                
                // Skip header row
                for (int i = 1; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(_separator);
                    
                    if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        recipients.Add(
                        parts[1].Trim())
                        ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading email recipients file: {ex.Message}", ex);
            }

            if (recipients.Count == 0)
            {
                throw new InvalidOperationException("No valid email recipients found in the configuration file.");
            }

            return recipients;
        }
    }
}