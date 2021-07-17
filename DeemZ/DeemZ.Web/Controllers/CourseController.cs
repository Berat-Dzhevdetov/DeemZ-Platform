namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.FormModels.Course;

    public class CourseController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(Guard guard, ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            this.guard = guard;
            this.courseService = courseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> ViewCourse(string courseId)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            var course = courseService.GetCourseById<DetailsCourseViewModel>(courseId);

            if (course == null) return NotFound();

            var user = await this.userManager.GetUserAsync(HttpContext.User);

            if (user == null) course.IsUserSignUpForThisCourse = false;
            else
            {
                course.IsUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(user.Id, courseId);
            }
            

            return View(course);
        }

        public IActionResult SignUp(string courseId)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(string courseId,SignUpCourseFormModel signUp)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var isUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(user.Id, courseId);

            if (isUserSignUpForThisCourse) return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });

            if (!ModelState.IsValid) return View(signUp);

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return BadRequest();

            courseService.SignUserToCourse(user.Id, course.Id);

            return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });
        }
    }
}