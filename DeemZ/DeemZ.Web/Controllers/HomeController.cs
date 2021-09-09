namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DeemZ.Models;
    using DeemZ.Models.DTOs;
    using DeemZ.Services.UserServices;
    using DeemZ.Web.Infrastructure;

    using static Global.WebConstants.Constants;
    using static Global.WebConstants.UserErrorMessages;

    public class HomeController : Controller
    {
        private readonly IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult UserErrorPage()
        {
            string errorMessageJson = null;
            try
            {
                errorMessageJson = TempData[ErrorMessageKey].ToString();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMessage = JsonConvert.DeserializeObject<ClientRequiredModel>(errorMessageJson);

            return View(errorMessage);
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

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}