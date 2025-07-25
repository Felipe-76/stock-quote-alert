namespace StockQuoteAlert.Services
{
    /// <summary>
    /// Service for sending email notifications.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an alert email to multiple recipients.
        /// </summary>
        /// <param name="recipients">List of email recipients.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="body">Email body.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendAlertAsync(List<string> recipients, string subject, string body);
    }
}