using HeyUrlChallengeCodeDotnet.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace hey_url_challenge_code_dotnet.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var model = new ErrorViewModel() 
            {
                StatusCode= statusCode,
            };
            switch (statusCode)
            {
                case 404:
                    model.ErrorMessage = "Uh oh, this page could not be found";
                    break;
                case 405:
                    model.ErrorMessage = "Method not allowed. Contact administrator";
                    break;
                case 401:
                    model.ErrorMessage = "You do not have acccess to this page. Please make sure you are logged in, or contact your administrator.";
                    break;
                case 500:
                    IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    model.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
                    break;
                case 403:
                    model.ErrorMessage = "Forbidden. Please contact administrator.";
                    break;
                case 503:
                    ViewBag.ErrorMessage = "Service unavailable. Please contact administrator";
                    break;
                case 504:
                    ViewBag.ErrorMessage = "Gateway Timeout. Please contact administrator";
                    break;
                case 001:
                    ViewBag.ErrorMessage = "This link has expired.";
                    break;

            }
            return View(model);
        }
    }
}
