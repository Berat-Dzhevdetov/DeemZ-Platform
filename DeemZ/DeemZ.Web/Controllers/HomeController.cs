namespace DeemZ.Web.Controllers
{
    using DeemZ.Models;
    using DeemZ.Services.UserServices;
    using DeemZ.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using static Global.WebConstants.Constants;
    using static Global.WebConstants.UserErrorMessages;

    public class HomeController : Controller
    {
        private readonly IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult UserErrorPage(string errorMsg)
        {
            if(!RouteData.Values.ContainsKey(ErrorMessageKey))
                return RedirectToAction(nameof(Index));

            var errorMessage = RouteData.Values[ErrorMessageKey];

            TempData[ErrorMessageKey] = errorMessage;
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

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}