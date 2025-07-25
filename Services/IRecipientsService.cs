namespace StockQuoteAlert.Services
{
    /// <summary>
    /// Service for managing email recipients configuration.
    /// </summary>
    public interface IRecipientsService
    {
        /// <summary>
        /// Gets the list of email recipients from configuration.
        /// </summary>
        /// <returns>List of email recipients.</returns>
        List<string> GetRecipients();
    }
}