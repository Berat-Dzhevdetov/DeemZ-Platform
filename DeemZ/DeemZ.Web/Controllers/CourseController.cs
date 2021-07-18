namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Web.Infrastructure;

    public class CourseController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;

        public CourseController(Guard guard, ICourseService courseService)
        {
            this.guard = guard;
            this.courseService = courseService;
        }

        public IActionResult ViewCourse(string courseId)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            var course = courseService.GetCourseById<DetailsCourseViewModel>(courseId);

            if (course == null) return NotFound();

            var userId = User.GetId();

            if (userId == null) course.IsUserSignUpForThisCourse = false;
            else
            {
                course.IsUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);
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
        public IActionResult SignUp(string courseId,SignUpCourseFormModel signUp)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            var userId = User.GetId();

            var isUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            if (isUserSignUpForThisCourse) return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });

            if (!ModelState.IsValid) return View(signUp);

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return BadRequest();

            courseService.SignUserToCourse(userId, course.Id);

            return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
    }
}