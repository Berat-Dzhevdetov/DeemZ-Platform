namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using PluralizeService.Core;
    using System;
    using System.Linq;
    using DeemZ.Global.Extensions;
    using DeemZ.Data;
    using DeemZ.Models.DTOs;
    using DeemZ.Models;
    using DeemZ.Web.Infrastructure;

    using static DeemZ.Global.WebConstants.UserErrorMessages;

    public class IfExists : ActionFilterAttribute
    {
        private DeemZDbContext context;
        private readonly string args;

        public IfExists(string args = null) => this.args = args;


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var argumentsToCheck = filterContext.ActionArguments.Where(x => x.Key.GetType().Name.ToLower() == "string"
                                                            && x.Key.ToLower().EndsWith("id")).ToList();

            if(args != null)
            {
                var tempArr = args.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();

                if(tempArr.Length >= 1)
                {
                    argumentsToCheck.Where(x => tempArr.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                }
            }


            if (argumentsToCheck.Count <= 0 || argumentsToCheck.Any(x => x.Value == null)) return;

            context = (DeemZDbContext)filterContext.HttpContext.RequestServices.GetService(typeof(DeemZDbContext));

            foreach (var arg in argumentsToCheck)
            {
                var resourceName = arg.Key.TrimEnd("Id");

                var pluralizedName = resourceName == "forum" ? "forums" : PluralizationProvider.Pluralize(resourceName);

                var tableName = pluralizedName.First().ToString().ToUpper() + pluralizedName[1..];

                try
                {
                    var result = context.FindEntity(tableName, arg.Value.ToString());

                    if (result != null) continue;

                    var errorModel = new HandleErrorModel
                    {
                        Message = string.Format(NotFoundMessage, resourceName),
                        StatusCode = HttpStatusCodes.NotFound
                    };

                    filterContext.RedirectToErrorPage(errorModel);
                }
                catch (Exception)
                {
                    var errorModel = new HandleErrorModel
                    {
                        Message = string.Format(InvalidParam, arg.Key),
                        StatusCode = HttpStatusCodes.BadRequest
                    };

                    filterContext.RedirectToErrorPage(errorModel);
                }
            }

        }
    }
}