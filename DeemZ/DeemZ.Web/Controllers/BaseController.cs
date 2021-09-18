namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using DeemZ.Models.DTOs;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models;

    using static Global.WebConstants.UserErrorMessages;

    public class BaseController : Controller
    {
        private static readonly Dictionary<HttpStatusCodes, HandleErrorModel> ErrorMessageDictionary = new()
        {
            {
                HttpStatusCodes.BadRequest,
                new HandleErrorModel
                {
                    Message = BadRequestMessage,
                    StatusCode = HttpStatusCodes.BadRequest
                }
            },
            {
                HttpStatusCodes.Forbidden,
                new HandleErrorModel
                {
                    Message = AccessDenied,
                    StatusCode = HttpStatusCodes.Forbidden
                }
            },
            {
                HttpStatusCodes.NotFound,
                new HandleErrorModel
                {
                    Message = NotFoundMessage,
                    StatusCode = HttpStatusCodes.NotFound
                }
            },
        };

        public IActionResult HandleErrorRedirect(HttpStatusCodes errorCode)
        {
            TempData[ErrorMessageKey] = JsonConvert.SerializeObject(ErrorMessageDictionary[errorCode]);

            return RedirectToAction(nameof(HomeController.UserErrorPage), typeof(HomeController).GetControllerName());
        }
    }
}