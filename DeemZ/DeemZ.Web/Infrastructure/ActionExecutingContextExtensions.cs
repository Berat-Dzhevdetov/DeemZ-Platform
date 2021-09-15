namespace DeemZ.Web.Infrastructure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Newtonsoft.Json;
    using DeemZ.Web.Controllers;
    using DeemZ.Models.DTOs;

    using static DeemZ.Global.WebConstants.UserErrorMessages;

    public static class ActionExecutingContextExtensions
    {
        public static void RedirectToErrorPage(this ActionExecutingContext filterContext, HandleErrorModel errorModel)
        {
            ((Controller)filterContext.Controller).TempData[ErrorMessageKey] = JsonConvert.SerializeObject(errorModel);

            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = typeof(HomeController).GetControllerName(), action = nameof(HomeController.UserErrorPage) })
                );
        }
    }
}