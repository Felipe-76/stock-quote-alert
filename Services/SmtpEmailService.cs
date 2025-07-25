using System.Net;
using System.Net.Mail;
using StockQuoteAlert.DataModels;
using System.Text.Json;

namespace StockQuoteAlert.Services
{
    /// <summary>
    /// Service for sending email alerts using SMTP.
    /// </summary>
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpConfig _config;

        /// <summary>
        /// Checks for valid emails.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailService"/> class.
        /// </summary>
        /// <param name="configFilePath">Path to the SMTP configuration JSON file.</param>
        public SmtpEmailService(string configFilePath = "smtpsettings.json")
        {
            _config = LoadSmtpConfig(configFilePath);
        }

        /// <summary>
        /// Loads SMTP configuration from JSON file.
        /// </summary>
        /// <param name="filePath">Path to the configuration file.</param>
        /// <returns>SMTP configuration.</returns>
        /// <exception cref="FileNotFoundException">Thrown when config file is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when config cannot be loaded.</exception>
        static private SmtpConfig LoadSmtpConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"SMTP configuration file not found: {filePath}");
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var settings = JsonSerializer.Deserialize<SmtpSettings>(jsonString);
                
                if (settings?.Smtp == null)
                {
                    throw new InvalidOperationException("Invalid SMTP configuration format.");
                }

                return settings.Smtp;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Error parsing SMTP configuration: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error loading SMTP configuration: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sends an email alert with the specified subject and body.
        /// </summary>
        /// <param name="recipients">List of email recipients.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="body">Email body.</param>
        public async Task SendAlertAsync(List<string> recipients, string subject, string body)
        {
            using var client = new SmtpClient(_config.Host, _config.Port)
            {
                Credentials = new NetworkCredential(_config.Username, _config.Password),
                EnableSsl = _config.EnableSsl ?? true
            };
            if (!IsValidEmail(_config.From))
            {
                Console.WriteLine($"Invalid 'From' email address in smtpsettings.json: {_config.From}");
                return;
            }

            foreach (var recipient in recipients)
            {
                if (!IsValidEmail(recipient))
                {
                    Console.WriteLine($"Invalid recipient email address: {recipient}");
                    continue;
                }

                var mail = new MailMessage(_config.From, recipient, subject, body);
                try
                {
                    await client.SendMailAsync(mail);
                    Console.WriteLine($"Alert sent successfully to {recipient}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send alert to {recipient}: {ex.Message}");
                }
                finally
                {
                    mail.Dispose();
                }
            }
        }
    }
}