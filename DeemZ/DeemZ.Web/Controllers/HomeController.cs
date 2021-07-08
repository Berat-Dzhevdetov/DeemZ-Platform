using AutoMapper;
using DeemZ.Data;
using DeemZ.Data.Models;
using DeemZ.Models;
using DeemZ.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using DeemZ.Models.ViewModels.Course;
using System.Collections.Generic;
using DeemZ.Models.ViewModels.Homework;
using DeemZ.Models.ViewModels.Resources;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DeemZ.Services.CourseServices;

namespace DeemZ.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DeemZDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ICourseService courseService;

        public HomeController(ILogger<HomeController> logger, DeemZDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, ICourseService courseService)
        {
            _logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.courseService = courseService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);

                var viewModel = new IndexUserViewModel()
                {
                    Credits = courseService.GetCreditsByUserId(user.Id),
                    Courses = courseService.GetCurrentCoursesByUserId<IndexCourseViewModel>(user.Id,true)
                };

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
