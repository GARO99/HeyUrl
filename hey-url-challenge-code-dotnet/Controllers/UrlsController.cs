using hey_url_challenge_code_dotnet.Rules.Contracts;
using hey_url_challenge_code_dotnet.Util.Exceptions;
using hey_url_challenge_code_dotnet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;
using System;

namespace HeyUrlChallengeCodeDotnet.Controllers
{
    [Route("/")]
    public class UrlsController : Controller
    {
        private readonly ILogger<UrlsController> _logger;
        private readonly Lazy<IUrlService> LazyUrlService;
        private readonly IBrowserDetector browserDetector;
        private IUrlService UrlService => LazyUrlService.Value;

        public UrlsController(ILogger<UrlsController> logger, IBrowserDetector browserDetector, Lazy<IUrlService> lazyUrlService)
        {
            this.browserDetector = browserDetector;
            _logger = logger;
            this.LazyUrlService = lazyUrlService;
        }

        public IActionResult Index()
        {
            TempData["baseUrl"] = $"{Request.Scheme}://{Request.Host}";
            return View(new HomeViewModel { Urls = this.UrlService.GetAll() });
        }

        [Route("urls/create")]
        public IActionResult Create([FromForm] string url)
        {
            try
            {
                this.UrlService.Create(url);
                TempData["baseUrl"] = $"{Request.Scheme}://{Request.Host}";
                return View("index", new HomeViewModel { Urls = this.UrlService.GetAll() });
            }
            catch (Exception ex)
            {
                TempData["heyUrlError"] = ex.Message;
                return View("index", new HomeViewModel { Urls = this.UrlService.GetAll() });
            }
        }

        [Route("/{url}")]
        public IActionResult Visit(string url) => new OkObjectResult($"{url}, {this.browserDetector.Browser.OS}, {this.browserDetector.Browser.Name}");

        [Route("urls/{url}")]
        public IActionResult Show(string url)
        {
            try
            {
                var model = new ShowViewModel
                {
                    Url = this.UrlService.GetByShortUrl(url)
                };

                return View(model);
            }
            catch (HeyUrlException ex)
            {
                TempData["heyUrlError"] = ex.Message;
                return View();
            }
            
            
        }
    }
}