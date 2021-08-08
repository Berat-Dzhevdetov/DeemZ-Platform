namespace DeemZ.Web.Controllers
{
    using DeemZ.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using DeemZ.Models;
    using DeemZ.Services.UserServices;

    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = User.GetId();

                var viewModel = userService.GetIndexInformaiton(userId);
                
                return View("LoggedIndex", viewModel);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}