using hey_url_challenge_code_dotnet.Rules.Contracts;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Shyjus.BrowserDetection;
using System;

namespace tests
{
    internal class CustomMock
    {
        public static HttpContext GetHttpContext()
        {
            Mock<HttpContext> httpContextMock = new();
            Mock<HttpRequest> httpRequestMock = new();
            httpRequestMock.Setup(req => req.Scheme).Returns("https");
            httpRequestMock.Setup(req => req.Host).Returns(new HostString("localhost:44327"));
            httpContextMock.Setup(ctx => ctx.Request).Returns(httpRequestMock.Object);

            return httpContextMock.Object;
        }

        public static ITempDataDictionary GetTempData()
        {
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            return tempData;
        }

        public static ILogger<UrlsController> GetUrlLogger()
        {
            Mock<ILogger<UrlsController>> loggerMock = new();

            return loggerMock.Object;
        }

        public static IBrowserDetector GetBrowserDetector()
        {
            Mock<IBrowserDetector> browserDetectorMock = new();

            return browserDetectorMock.Object;
        }

        public static Lazy<IUrlService> GetLazyUrlService()
        {
            Mock<IUrlService> urlServiceMock= new();

            return new Lazy<IUrlService>(() => urlServiceMock.Object);
        }
    }
}
