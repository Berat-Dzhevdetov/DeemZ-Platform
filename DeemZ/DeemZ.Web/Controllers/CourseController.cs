namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class CourseController : Controller
    {
        public IActionResult ViewCourse(string courseId)
        {
            if (courseId == null) return NotFound();

            return View();
        }
    }
}
