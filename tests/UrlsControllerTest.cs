using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrlChallengeCodeDotnet.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

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
            Assert.IsNotNull(viewResult.TempData["baseUrl"]);
            Assert.IsNotEmpty(viewResult.TempData["baseUrl"].ToString());
            Assert.IsInstanceOf<HomeViewModel>(viewResult.Model);
            HomeViewModel model = viewResult.Model as HomeViewModel;
            Assert.IsInstanceOf<IEnumerable<Url>>(model.Urls);
        }

        [Test]
        [TestCase("https://www.youtube.com/")]
        [TestCase("asd")]
        public void TestCreate(string url)
        {
            IActionResult result = this.Controller.Create(url);
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.IsInstanceOf<HomeViewModel>(viewResult.Model);
            HomeViewModel model = viewResult.Model as HomeViewModel;
            Assert.IsInstanceOf<IEnumerable<Url>>(model.Urls);
            Assert.AreEqual("index", viewResult.ViewName);
            if (url == "https://www.youtube.com/")
            {
                Assert.IsNotNull(viewResult.TempData["baseUrl"]);
                Assert.IsNotEmpty(viewResult.TempData["baseUrl"].ToString());
            }
            else
            {
                Assert.IsNotNull(viewResult.TempData["heyUrlError"]);
                Assert.IsNotEmpty(viewResult.TempData["heyUrlError"].ToString());
            }
        }

        [Test]
        [TestCase("WEIOP")]
        [TestCase("EROPL")]
        public void TestVisit(string url)
        {
            IActionResult result = this.Controller.Visit(url);
            if (url == "WEIOP")
            {
                Assert.IsInstanceOf<RedirectResult>(result);
            }
            else
            {
                Assert.IsInstanceOf<RedirectToActionResult>(result);
                RedirectToActionResult actionResult = result as RedirectToActionResult;
                Assert.AreEqual("Error", actionResult.ActionName);
                Assert.AreEqual(404, actionResult.RouteValues["id"]);
            }
        }

        [Test]
        [TestCase("WEIOP")]
        [TestCase("EROPL")]
        public void TestShow(string url)
        {
            IActionResult result = this.Controller.Show(url);
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual(null, viewResult.ViewName);
            if (url == "WEIOP")
            {
                Assert.IsInstanceOf<ShowViewModel>(viewResult.Model);
                ShowViewModel model = viewResult.Model as ShowViewModel;
                Assert.IsInstanceOf<Url>(model.Url);
            }
            else
            {
                Assert.IsNull(viewResult.Model);
                Assert.IsNotNull(viewResult.TempData["heyUrlError"]);
                Assert.IsNotEmpty(viewResult.TempData["heyUrlError"].ToString());
            }
        }
    }
}