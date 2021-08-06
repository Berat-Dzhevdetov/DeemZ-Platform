namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.LectureServices;

    using static DeemZ.Global.WebConstants.Constants;
    using static Infrastructure.ControllerExtensions;

    public class CourseController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;
        private readonly ILectureService lectureService;

        public CourseController(Guard guard, ICourseService courseService, ILectureService lectureService)
        {
            this.guard = guard;
            this.courseService = courseService;
            this.lectureService = lectureService;
        }

        public IActionResult ViewCourse(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            var course = courseService.GetCourseById<DetailsCourseViewModel>(courseId);

            var userId = User.GetId();

            if (userId == null) course.IsUserSignUpForThisCourse = false;
            else course.IsUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            return View(course);
        }

        public IActionResult SignUp(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SignUp(string courseId, SignUpCourseFormModel signUp)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var userId = User.GetId();

            var isUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            if (isUserSignUpForThisCourse) return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });

            if (!ModelState.IsValid) return View(signUp);

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return BadRequest();

            courseService.SignUserToCourse(userId, course.Id);

            return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add() => View();


        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public IActionResult Add(AddCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            var courseId = courseService.CreateCourse(course);

            if (course.BasicLectures) courseService.CreateBasicsLectures(courseId,course);

            if (course.Redirect) return RedirectToAction(nameof(ViewCourse), new { courseId });

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName());
        }

        [Authorize]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var course = courseService.GetCourseById<EditCourseFormModel>(courseId);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public IActionResult Edit(string courseId, EditCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            courseService.EditCourseById(course, courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName());
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Delete(string courseId)
        {
            if (guard.AgainstNull(courseId)) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            courseService.DeleteCourse(courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName());
        }
    }
}