using System;
using DeemZ.Global.WebConstants;
using Microsoft.AspNetCore.Authorization;

namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DeemZ.Models;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.UserServices;

    using static Global.WebConstants.Constants;

    public class HomeController : Controller
    {
        private readonly IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult UserErrorPage(string errorMsg)
        {
            ViewData["Error"] = errorMsg;
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = User.GetId();

                var isAdmin = await userService.IsInRoleAsync(userId, AdminRoleName);

                var viewModel = userService.GetIndexInformaiton(userId, !isAdmin);
                
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