namespace DeemZ.Web.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using DeemZ.Models.DTOs.IpLogg;

    using static DeemZ.Infrastructure.Secret.IpLoggSetup;

    public class IpLogger : ActionFilterAttribute
    {
        private const string URL = "https://api.ipdata.co?api-key=";
        private readonly string targetUrl = $"{URL}{key}";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(targetUrl).Result;

            if (!response.IsSuccessStatusCode) throw new ArgumentNullException("Api key is empty or null");

            // Parse the response body.
            var json = response.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<IpLoggDTO>(json);

            Console.WriteLine(data.Ip);
            Console.WriteLine(data.City);
        }
    }
}
