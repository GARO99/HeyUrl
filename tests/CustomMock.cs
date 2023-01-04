using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Rules.Contracts;
using hey_url_challenge_code_dotnet.Util.Exceptions;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;

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
            urlServiceMock.Setup(us => us.GetAll()).Returns(new List<Url>());
            urlServiceMock.Setup(us => us.Create("https://www.youtube.com/")).Returns(new Url());
            urlServiceMock.Setup(us => us.Create("asd")).Throws(new HeyUrlException("Submited url is invalid"));
            urlServiceMock.Setup(us => us.AddClick(It.IsAny<IBrowserDetector>(), "WEIOP")).Returns(new Url { OriginalUrl = "https://www.youtube.com/" });
            urlServiceMock.Setup(us => us.AddClick(It.IsAny<IBrowserDetector>(), "EROPL")).Throws(new HeyUrlException("URL dosen't exist"));
            urlServiceMock.Setup(us => us.GetByShortUrl("WEIOP")).Returns(new Url());
            urlServiceMock.Setup(us => us.GetByShortUrl("EROPL")).Throws(new HeyUrlException("The Url you are looking for does not exist"));


            return new Lazy<IUrlService>(() => urlServiceMock.Object);
        }
    }
}
