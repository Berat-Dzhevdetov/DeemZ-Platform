using AutoMapper;
using DeemZ.Data;
using DeemZ.Data.Models;
using DeemZ.Web.Models;
using DeemZ.Web.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using DeemZ.Web.Models.ViewModels.Course;
using System.Collections.Generic;
using DeemZ.Web.Models.ViewModels.Homework;
using DeemZ.Web.Models.ViewModels.Resources;

namespace DeemZ.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DeemZDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, DeemZDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var user = this.userManager.GetUserAsync(HttpContext.User);
                var viewModel = new IndexUserViewModel()
                {
                    Courses = new List<IndexCourseViewModel>(),
                    Homework = new List<IndexHomeworkViewModel>(),
                    Resources = new List<IndexResourceViewModel>(),
                    Credits = 89
                };
                //var viewModel = mapper.Map<IndexUserViewModel>(user);
                return this.View("LoggedIndex", viewModel);
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
