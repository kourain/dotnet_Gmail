using System.Text.Json.Serialization;

namespace GmailService.Classes
{
    public class EmailRequest
    {
        [JsonPropertyName("To")]
        public List<string> To { get; set; }
        [JsonPropertyName("Cc")]
        public List<string> Cc { get; set; } = new List<string>();
        [JsonPropertyName("Bcc")]
        public List<string> Bcc { get; set; } = new List<string>();
        [JsonPropertyName("Subject")]
        public string Subject { get; set; }
        [JsonPropertyName("Body")]
        public string Body { get; set; }
        [JsonPropertyName("IsHtml")]
        public bool IsHtml { get; set; } = true;
        [JsonPropertyName("Attachments")]
        public List<EmailAttachment>? Attachments { get; set; }
        public List<string>? Validate()
        {
            var errors = new List<string>();
            if (To == null || !To.Any())
            {
                errors.Add("At least one recipient (To) is required.");
            }

            if (string.IsNullOrWhiteSpace(Subject))
            {
                errors.Add("Email subject is required.");
            }

            if (string.IsNullOrWhiteSpace(Body))
            {
                errors.Add("Email body is required.");
            }
            return errors.Count > 0 ? errors : null;
        }
    }

    public class EmailAttachment
    {
        [JsonPropertyName("FileName")]
        public string FileName { get; set; }
        [JsonPropertyName("ContentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("Content")]
        public byte[] Content { get; set; }
    }
}