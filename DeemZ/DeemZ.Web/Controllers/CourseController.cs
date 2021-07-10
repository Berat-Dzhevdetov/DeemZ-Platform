namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;

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

            return View(course);
        }
    }
}