namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using DeemZ.Services;
    using DeemZ.Models.DTOs;
    using DeemZ.Models;
    using DeemZ.Web.Controllers;
    using DeemZ.Web.Infrastructure;

    using static DeemZ.Global.WebConstants.UserErrorMessages;

    public class ClientRequired : ActionFilterAttribute
    {
        private string args = null;
        private readonly Guard guard;

        public ClientRequired(string args = null)
        {
            guard = new Guard();
            this.args = args;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionArguments = filterContext.ActionArguments;

            if (args != null)
            {
                var tempArr = args.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var arg in tempArr)
                {
                    if (!actionArguments.Keys.Contains(arg))
                    {
                        var controllerDescription = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor;
                        var actionName = controllerDescription.ActionName;

                        var controllerName = controllerDescription.ControllerName;

                        var requestMethod = filterContext.HttpContext.Request.Method;

                        throw new ArgumentException($"Invalid argument '{arg}' in '{controllerName}/{actionName}' while trying to do '{requestMethod}' request");
                    }
                }
            }

            var nullValue = CheckAgainstNull(actionArguments, filterContext);
            if (nullValue != null)
            {
                var clientRequired = new ClientRequiredModel
                {
                    Message = string.Format(InvalidParam, nullValue),
                    StatusCode = HttpStatusCodes.BadRequest
                };

                ((Controller)filterContext.Controller).TempData[ErrorMessageKey] = JsonConvert.SerializeObject(clientRequired);

                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = typeof(HomeController).GetControllerName(), action = nameof(HomeController.UserErrorPage) })
                    );
            }
        }

        private string CheckAgainstNull(IDictionary<string, object> actionArguments, ActionExecutingContext filterContext)
        {
            foreach (KeyValuePair<string, object> entry in actionArguments)
                if (guard.AgainstNull(entry.Value, nameof(entry.Value)))
                    return entry.Key;
            return null;
        }
    }
}