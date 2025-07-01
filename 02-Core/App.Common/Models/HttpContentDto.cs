using System.Collections.Generic;

namespace App.Common.Models
{
    public sealed class HttpContentDto
    {
        public object Data { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}
