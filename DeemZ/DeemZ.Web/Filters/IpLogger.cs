namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using DeemZ.Models.DTOs.IpLogg;
    using DeemZ.Services.LocationServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.IpLoggServices;
    using DeemZ.Data;
    using DeemZ.Services.EmailSender;
    using DeemZ.Data.Models;

    using static DeemZ.Infrastructure.Secret.IpLoggSetup;
    using static DeemZ.Global.WebConstants.Constant;

    public class IpLogger : ActionFilterAttribute
    {
        private const string URL = "https://api.ipdata.co?api-key=";
        private readonly string targetUrl = $"{URL}{key}";
        private DeemZDbContext context;
        private readonly string subjectForEmail = "Access from new web or mobile device";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated || IsAjaxRequest(filterContext.HttpContext.Request))
                return;

            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(targetUrl).Result;

            if (!response.IsSuccessStatusCode)
                throw new ArgumentNullException("Request to apidata failed");

            // Parse the response body.
            var json = response.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<IpLoggDTO>(json);

            context = filterContext.HttpContext.RequestServices.GetService<DeemZDbContext>();
            var locationSerivce = (ILocationService)Activator.CreateInstance(typeof(LocationService), new object[] { context });

            if (!locationSerivce.LocationExists(data.Country_Name, data.Region, data.City).Result)
            {
                locationSerivce.CreateLocation(data.Country_Name, data.Region, data.City).Wait();
            }

            var userId = filterContext.HttpContext.User.GetId();

            // Check what was the last ip was logged with
            var ipLoggerService = (IIpLoggerService)Activator.CreateInstance(typeof(IpLoggerService), new object[] { context, locationSerivce });

            if (!ipLoggerService.IsNotFirstEnter(userId))
            {
                ipLoggerService.SaveIpToUser(userId, data.Ip, data.City);
            }
            else if (ipLoggerService.IsTheSameIpAsLast(userId, data.Ip).Result)
            {
                var emailSenderService = (IEmailSenderService)Activator.CreateInstance(typeof(EmailSenderService), new object[] { context });
                var userManager = filterContext.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

                var user = userManager.FindByIdAsync(userId).Result;

                // send email to user
                var htmlContent = $@"<div>Hello <b><i>{user.FirstName} {user.LastName}</b></i>,</div><br>

<div>It looks like you are trying to log in from a new device. Here is some addition information:</div>
<div>IP: <b>{data.Ip}</b></div>
<div>City: <b>{data.City}</b></div>
<div>Region: <b>{data.Region}</b></div>
<div>Country: <b>{data.Country_Name}</b></div>
<div>Time: <b>{DateTime.Now.ToString(DateTimeFormat)}</b></div><br>

<div>If that was you just ignore this message, otherwise please click <a target='_blank' href='we.com'>Here</a></div>";
                emailSenderService.SendEmailAsync(user.Email, subjectForEmail, htmlContent).Wait();
                // save the new ip
                ipLoggerService.SaveIpToUser(userId, data.Ip, data.City);
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
