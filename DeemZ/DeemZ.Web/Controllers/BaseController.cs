using DeemZ.Models.DTOs;
using DeemZ.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeemZ.Web.Controllers
{
    using System.Collections.Generic;
    using static Global.WebConstants.Constants;
    using static Global.WebConstants.UserErrorMessages;

    public class BaseController : Controller
    {
        private static Dictionary<int, HandleErrorModel> ErrorMessageDictionary = new Dictionary<int, HandleErrorModel>()
        {
            {401, new HandleErrorModel
            {
                Message = BadRequestMessage,
                StatusCode = Models.HttpStatusCodes.BadRequest
            }},
            {403,new HandleErrorModel
            {
                 Message = AccessDenied,
                 StatusCode = Models.HttpStatusCodes.Forbidden
            }},
            {404,new HandleErrorModel
            {
                 Message = NotFoundMessage,
                 StatusCode = Models.HttpStatusCodes.NotFound
            }},
        };

        public IActionResult HandleErrorRedirect(int errorCode)
        {
            TempData[ErrorMessageKey] = JsonConvert.SerializeObject(ErrorMessageDictionary[errorCode]);

            return RedirectToAction(nameof(HomeController.UserErrorPage), typeof(HomeController).GetControllerName());
        }
    }
}
