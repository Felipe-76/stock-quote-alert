namespace StockQuoteAlert.DataModels
{
    public class SmtpSettings
    {
        required public SmtpConfig Smtp { get; set; }
    }

    public class SmtpConfig
    {
        required public string Host { get; set; }
        required public int Port { get; set; }
        public bool? EnableSsl { get; set; }
        required public string Username { get; set; }
        required public string Password { get; set; }
        required public string From { get; set; }
    }
}