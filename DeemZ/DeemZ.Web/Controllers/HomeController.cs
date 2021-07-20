namespace DeemZ.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using DeemZ.Models;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.User;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.ViewModels.Resources;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.ResourceService;

    public class HomeController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ISurveyService serveyService;
        private readonly IResourceService resourceService;

        public HomeController(ICourseService courseService, ISurveyService serveyService, IResourceService resourceService)
        {
            this.courseService = courseService;
            this.serveyService = serveyService;
            this.resourceService = resourceService;
        }

        public IActionResult Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = User.GetId();

                var viewModel = new IndexUserViewModel()
                {
                    Credits = courseService.GetUserCredits(userId),
                    Courses = courseService.GetUserCurrentCourses<IndexCourseViewModel>(userId, true),
                    Surveys = serveyService.GetUserCurrentCourseSurveys<IndexSurveyViewModel>(userId),
                    Resources = resourceService.GetUserResources<IndexResourceViewModel>(userId),
                    SignUpCourses = courseService.GetCoursesForSignUp<IndexSignUpForCourseViewModel>()
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
