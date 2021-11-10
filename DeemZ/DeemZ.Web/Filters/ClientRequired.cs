namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using DeemZ.Services;
    using DeemZ.Models.DTOs;
    using DeemZ.Models;
    using DeemZ.Web.Infrastructure;

    using static DeemZ.Global.WebConstants.UserErrorMessages;
    using System.Linq;

    public class ClientRequired : ActionFilterAttribute
    {
        private readonly string args = null;
        private readonly Guard guard;

        public ClientRequired(string args = null)
        {
            guard = new Guard();
            this.args = args;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionArguments = filterContext.ActionArguments;
            var actualActionArguments = filterContext.ActionDescriptor.Parameters;

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
            //Checking if there is no needed argument
            else if (actionArguments.Count != actualActionArguments.Count)
            {
                var errorModel = new HandleErrorModel
                {
                    Message = WrongExpectedParams,
                    StatusCode = HttpStatusCodes.BadRequest
                };

                filterContext.RedirectToErrorPage(errorModel);
            }

            var nullValue = CheckAgainstNull(actionArguments);

            if (nullValue != null)
            {
                var errorModel = new HandleErrorModel
                {
                    Message = string.Format(InvalidParam, nullValue),
                    StatusCode = HttpStatusCodes.BadRequest
                };

                filterContext.RedirectToErrorPage(errorModel);
            }
        }

        private string CheckAgainstNull(IDictionary<string, object> actionArguments)
        {
            foreach (KeyValuePair<string, object> entry in actionArguments)
                if (guard.AgainstNull(entry.Value, nameof(entry.Value)))
                    return entry.Key;
            return null;
        }
    }
}