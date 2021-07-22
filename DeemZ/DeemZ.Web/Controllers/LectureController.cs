namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Lecture;

    //For Admins only
    [Authorize]
    public class LectureController : Controller
    {
        private readonly Guard guard;
        private readonly ILectureService lectureService;
        private readonly ICourseService courseService;

        public LectureController(Guard guard, ILectureService lectureService, ICourseService courseService)
        {
            this.guard = guard;
            this.lectureService = lectureService;
            this.courseService = courseService;
        }

        public IActionResult Add(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            return View();
        }

        [HttpPost]
        public IActionResult Add(string courseId, AddLectureFormModel lecture)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            if (!ModelState.IsValid) return View(lecture);

            lectureService.AddLectureToCourse(courseId,lecture);

            return RedirectToAction(nameof(AdministrationController.Lectures), "AdministrationController", new { courseId });
        }
    }
}