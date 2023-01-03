#nullable enable
using System;

namespace hey_url_challenge_code_dotnet.Models
{
    public class DailyUrlClick
    {
        public Guid Id { get; set; }
        public Guid UrlId { get; set; }
        public int DayNumber { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual Url? Url { get; set; }
    }
}
