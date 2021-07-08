using DeemZ.Data;
using DeemZ.Data.Models;
using DeemZ.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeemZ.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DeemZDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, DeemZDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);
                return this.View("LoggedIndex",user);
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
