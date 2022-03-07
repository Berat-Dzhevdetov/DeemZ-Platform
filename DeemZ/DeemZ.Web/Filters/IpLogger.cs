namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using DeemZ.Models.DTOs.IpLogg;
    using DeemZ.Services.LocationServices;
    using DeemZ.Data.Models;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.IpLoggServices;

    using static DeemZ.Infrastructure.Secret.IpLoggSetup;

    public class IpLogger : ActionFilterAttribute
    {
        private const string URL = "https://api.ipdata.co?api-key=";
        private readonly string targetUrl = $"{URL}{key}";
        private ILocationService locationSerivce;
        private IIpLoggerService ipLoggerService;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated || IsAjaxRequest(filterContext.HttpContext.Request))
                return;


            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(targetUrl).Result;

            if (!response.IsSuccessStatusCode)
                throw new ArgumentNullException("Request to apidata failed");

            // Parse the response body.
            var json = response.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<IpLoggDTO>(json);

            locationSerivce = filterContext.HttpContext.RequestServices.GetService<LocationService>();

            if (!locationSerivce.LocationExists(data.Country_Name, data.Region, data.City).Result)
            {
                locationSerivce.CreateLocation(data.Country_Name, data.Region, data.City).Wait();
            }

            var userId = filterContext.HttpContext.User.GetId();

            // Check what was the last ip was logged with
            ipLoggerService = filterContext.HttpContext.RequestServices.GetService<IpLoggerService>();

            if (ipLoggerService.IsTheSameIpAsLast(userId, data.Ip).Result)
            {
                // send email to user
                // save the new ip
            }
        }

        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
