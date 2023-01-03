using hey_url_challenge_code_dotnet.Rules.Contracts;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shyjus.BrowserDetection;
using System;

namespace tests
{
    public class UrlsControllerTest
    {
        private UrlsController Controller { get; set; }

        [SetUp]
        public void Setup()
        {
            this.Controller = new UrlsController(CustomMock.GetUrlLogger(), CustomMock.GetBrowserDetector(), CustomMock.GetLazyUrlService());
            this.Controller.ControllerContext.HttpContext = CustomMock.GetHttpContext();
            this.Controller.TempData = CustomMock.GetTempData();
        }

        [Test]
        public void TestIndex()
        {
            IActionResult result = this.Controller.Index();
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual(null, viewResult.ViewName);
        }
    }
}