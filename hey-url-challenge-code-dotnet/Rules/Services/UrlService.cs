using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Rules.Contracts;
using hey_url_challenge_code_dotnet.Util;
using hey_url_challenge_code_dotnet.Util.Exceptions;
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
            Url urlRegister = this.DbContext.Urls.FirstOrDefault(u => u.ShortUrl == url);
            if (urlRegister == null)
            {
                throw new HeyUrlException("URL dosen't exist");
            }
            urlRegister.ClickCount++;
            this.AddDailyUrlClick(urlRegister.Id);
            this.AddPlatformClick(urlRegister.Id, browserDetector.Browser.OS);
            this.AddBrowseClick(urlRegister.Id, browserDetector.Browser.Name);
            this.DbContext.SaveChanges();
            this.DbContext.Entry(urlRegister).Collection(c => c.DailyUrlClick).Load();
            this.DbContext.Entry(urlRegister).Collection(c => c.BrowseUrlClick).Load();
            this.DbContext.Entry(urlRegister).Collection(c => c.PlatformUrlClick).Load();

            return urlRegister;
        }

        private void AddDailyUrlClick(Guid urlId)
        {
            DailyUrlClick dailyUrlClick = this.DbContext.DailyUrlClicks.FirstOrDefault(d => d.UrlId == urlId && d.CreateAt.Date == DateTime.Now.Date);
            if (dailyUrlClick != null)
            {
                dailyUrlClick.ClickCount++;
            }
            else
            {
                int lastDay = 0;
                if (this.DbContext.DailyUrlClicks.Any())
                {
                    lastDay = this.DbContext.DailyUrlClicks.Max(p => p.DayNumber);
                }
                this.DbContext.DailyUrlClicks.Add(new DailyUrlClick
                {
                    UrlId = urlId,
                    ClickCount = 1,
                    DayNumber = lastDay + 1,
                    CreateAt = DateTime.Now
                });
            }
        }

        private void AddPlatformClick(Guid urlId, string paltformName)
        {
            PlatformUrlClick platformUrlClick = this.DbContext.PlatformUrlClicks.FirstOrDefault(p => p.UrlId == urlId && p.Name == paltformName);
            if (platformUrlClick != null)
            {
                platformUrlClick.ClickCount++;
            }
            else
            {
                this.DbContext.PlatformUrlClicks.Add(new PlatformUrlClick
                {
                    UrlId = urlId,
                    ClickCount = 1,
                    Name = paltformName,
                });
            }
        }

        private void AddBrowseClick(Guid urlId, string browserName)
        {
            BrowseUrlClick browseUrlClick = this.DbContext.BrowseUrlClicks.FirstOrDefault(b => b.UrlId == urlId && b.Name == browserName);
            if (browseUrlClick != null) 
            {
                browseUrlClick.ClickCount++;
            }
            else
            {
                this.DbContext.BrowseUrlClicks.Add(new BrowseUrlClick
                {
                    UrlId = urlId,
                    ClickCount = 1,
                    Name = browserName,
                });
            }
        }

        public Url Create(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                if (this.DbContext.Urls.FirstOrDefault(u => u.OriginalUrl == url) != null)
                {
                    throw new HeyUrlException("Long URL already registered");
                }
                string shortUrl = ShortUrl.Generated(length: 5);
                while (this.DbContext.Urls.FirstOrDefault(u => u.ShortUrl == shortUrl) != null)
                {
                    shortUrl = ShortUrl.Generated(length: 5);
                }
                this.DbContext.Urls.Add(new()
                {
                    OriginalUrl = url,
                    ShortUrl = shortUrl,
                    ClickCount = 0,
                    CreateAt = DateTime.Now
                });
                this.DbContext.SaveChanges();
                Url newUrl = this.DbContext.Urls.FirstOrDefault(u => u.ShortUrl == shortUrl);
                this.DbContext.Entry(newUrl).Collection(c => c.DailyUrlClick).Load();
                this.DbContext.Entry(newUrl).Collection(c => c.BrowseUrlClick).Load();
                this.DbContext.Entry(newUrl).Collection(c => c.PlatformUrlClick).Load();

                return newUrl;
            }
            else
            {
                throw new HeyUrlException("Submited url is invalid");
            }
        }

        public ICollection<Url> GetAll()
        {
            ICollection<Url> urls = this.DbContext.Urls.ToList();
            foreach (Url url in urls)
            {
                this.DbContext.Entry(url).Collection(u => u.DailyUrlClick).Load();
                this.DbContext.Entry(url).Collection(u => u.BrowseUrlClick).Load();
                this.DbContext.Entry(url).Collection(u => u.PlatformUrlClick).Load();
            }

            return urls;
        }

        public Url GetByShortUrl(string shortUrl)
        {
            Url url = this.DbContext.Urls.FirstOrDefault(u => u.ShortUrl == shortUrl);
            if (url == null)
            {
                throw new HeyUrlException("The Url you are looking for does not exist");
            }
            this.DbContext.Entry(url).Collection(u => u.DailyUrlClick).Load();
            this.DbContext.Entry(url).Collection(u => u.BrowseUrlClick).Load();
            this.DbContext.Entry(url).Collection(u => u.PlatformUrlClick).Load();

            return url;
        }
    }
}
