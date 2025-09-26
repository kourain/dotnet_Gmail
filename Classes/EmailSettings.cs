using System.Text.Json.Serialization;

namespace GmailService.Classes
{
    public class EmailSettings
    {
        [JsonPropertyName("SmtpServer")]
        public string SmtpServer { get; set; }
        [JsonPropertyName("SmtpPort")]
        public int SmtpPort { get; set; }
        [JsonPropertyName("SmtpUsername")]
        public string SmtpUsername { get; set; }
        [JsonPropertyName("SmtpPassword")]
        public string SmtpPassword { get; set; }
        [JsonPropertyName("SenderEmail")]
        public string SenderEmail { get; set; }
        [JsonPropertyName("SenderName")]
        public string SenderName { get; set; }
    }
}