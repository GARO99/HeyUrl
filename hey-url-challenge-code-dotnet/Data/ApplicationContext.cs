using hey_url_challenge_code_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace HeyUrlChallengeCodeDotnet.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }
        public DbSet<DailyUrlClick> DailyUrlClicks { get; set; }
        public DbSet<BrowseUrlClick> BrowseUrlClicks { get; set; }
        public DbSet<PlatformUrlClick> PlatformUrlClicks { get; set; }
    }
}