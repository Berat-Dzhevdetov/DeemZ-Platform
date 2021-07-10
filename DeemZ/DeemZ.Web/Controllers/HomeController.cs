namespace DeemZ.App.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DeemZ.Data.Models;
    using DeemZ.Models;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.User;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Survey;

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICourseService courseService;
        private readonly ISurveyService serveyService;

        public HomeController(UserManager<ApplicationUser> userManager, ICourseService courseService, ISurveyService serveyService)
        {
            this.userManager = userManager;
            this.courseService = courseService;
            this.serveyService = serveyService;
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
                    Courses = courseService.GetUserCurrentCourses<IndexCourseViewModel>(user.Id,true),
                    Surveys = serveyService.GetUserCurrentCourseSurveys<IndexSurveyViewModel>(user.Id)
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
