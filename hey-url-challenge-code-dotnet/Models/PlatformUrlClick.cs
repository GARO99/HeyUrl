#nullable enable
using System;

namespace hey_url_challenge_code_dotnet.Models
{
    public class PlatformUrlClick
    {
        public Guid Id { get; set; }
        public Guid UrlId { get; set; }
        public string Name { get; set; } = null!;
        public int ClickCount { get; set; }
        public virtual Url? Url { get; set; }
    }
}
