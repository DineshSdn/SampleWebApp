using System.Collections.Generic;

namespace App.Common.Models
{
    public sealed class EmailOptionsDto
    {
        public string SenderDisplayName { get; set; } = "Support Team";
        public IEnumerable<string> RecipientEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
