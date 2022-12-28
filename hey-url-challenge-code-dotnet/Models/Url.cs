using System;
using System.Collections.Generic;

namespace hey_url_challenge_code_dotnet.Models
{
    public class Url
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
        public int ClickCount { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<DailyUrlClick> DailyUrlClick { get; set; } = new List<DailyUrlClick>();
        public virtual ICollection<BrowseUrlClick> BrowseUrlClick { get; set; } = new List<BrowseUrlClick>();
        public virtual ICollection<PlatformUrlClick> PlatformUrlClick { get; set; } = new List<PlatformUrlClick>();
    }
}
