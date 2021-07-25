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
    using DeemZ.Models.FormModels.Lecture;

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

        //for admin only
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(AddCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            var courseId = courseService.CreateCourse(course);

            if (course.BasicLectures)
            {
                var basicLecturesNames = new string[] { "Resources", "Course Introduciton" };

                for (int i = 0; i < basicLecturesNames.Length; i++)
                {
                    var lecture = new AddLectureFormModel
                    {
                        Date = course.StartDate.AddSeconds(i),
                        Name = basicLecturesNames[i]
                    };

                    lectureService.AddLectureToCourse(courseId, lecture);
                }
            }

            if (course.Redirect) return RedirectToAction(nameof(ViewCourse), new { courseId });
            
            return RedirectToAction(nameof(AdministrationController.Courses), "Administration");
        }

        [Authorize]
        public IActionResult Edit(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var course = courseService.GetCourseById<EditCourseFormModel>(courseId);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(string courseId, EditCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            courseService.EditCourseById(course, courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), "Administration");
        }
    }
}