using hey_url_challenge_code_dotnet.Models;
using Shyjus.BrowserDetection;
using System.Collections.Generic;

namespace hey_url_challenge_code_dotnet.Rules.Contracts
{
    public interface IUrlService
    {
        ICollection<Url> GetAll();
        Url GetByShortUrl(string shortUrl);
        Url Create(string url);
        void AddClick(IBrowserDetector browserDetector, string url);
    }
}
