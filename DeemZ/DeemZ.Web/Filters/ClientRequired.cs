﻿namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Routing;
    using System.Linq;
    using System;
    using DeemZ.Web.Controllers;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services;

    using static DeemZ.Global.WebConstants.UserErrorMessages;
    using System.Diagnostics;

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

            if(args != null)
            {
                var tempArr = args.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var arg in tempArr)
                {
                    if (!actionArguments.Keys.Contains(arg))
                    {
                        var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName;

                        var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName;

                        var requestMethod = filterContext.HttpContext.Request.Method;

                        throw new ArgumentException($"Invalid argument '{arg}' in '{controllerName}/{actionName}' while trying to do '{requestMethod}' request");
                    }
                }

                actionArguments = actionArguments.Where(x => tempArr.Contains(x.Value)).ToDictionary(x => x.Key, x => x.Value);
            }

            CheckAgainstNull(actionArguments, filterContext);
        }

        private bool CheckAgainstNull(IDictionary<string,object> actionArguments, ActionExecutingContext filterContext)
        {
            foreach (KeyValuePair<string, object> entry in actionArguments)
            {
                if (guard.AgainstNull(entry.Value, nameof(entry.Value)))
                {
                    filterContext.RouteData.Values.Add(ErrorMessageKey, string.Format(InvalidParam, entry.Key.ToLower()));

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = typeof(HomeController).GetControllerName(), action = nameof(HomeController.UserErrorPage) })
                    );
                    return false;
                }
            }
            return true;
        }
    }
}