using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Rules.Contracts;
using HeyUrlChallengeCodeDotnet.Data;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hey_url_challenge_code_dotnet.Rules.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApplicationContext DbContext;

        public UrlService(ApplicationContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public Url AddClick(IBrowserDetector browserDetector, string url)
        {
            throw new System.NotImplementedException();
        }

        public Url Create(string url)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Url> GetAll()
        {
            ICollection<Url> urls = this.DbContext.Urls.ToList();
            foreach (Url url in urls)
            {
                this.DbContext.Entry(url).Reference(u => u.DailyUrlClick).Load();
                this.DbContext.Entry(url).Reference(u => u.BrowseUrlClick).Load();
                this.DbContext.Entry(url).Reference(u => u.PlatformUrlClick).Load();
            }

            return urls;
        }

        public Url GetByShortUrl(string shortUrl)
        {
            Url url = this.DbContext.Urls.FirstOrDefault(u => u.ShortUrl == shortUrl);
            if (url == null)
            {
                throw new Exception("The Url you are looking for does not exist");
            }
            this.DbContext.Entry(url).Reference(u => u.DailyUrlClick).Load();
            this.DbContext.Entry(url).Reference(u => u.BrowseUrlClick).Load();
            this.DbContext.Entry(url).Reference(u => u.PlatformUrlClick).Load();

            return url;
        }
    }
}
