namespace DeemZ.Web.Controllers
{
    using DeemZ.Services;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : Controller
    {
        private readonly Guard guard;

        public CourseController(Guard guard)
        {
            this.guard = guard;
        }

        public IActionResult ViewCourse(string courseId)
        {
            if (guard.AgainstNull(courseId)) return NotFound();

            return View();
        }
    }
}