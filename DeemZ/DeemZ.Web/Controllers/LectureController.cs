namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Models.FormModels.Description;

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

            return RedirectToAction(nameof(AdministrationController.Lectures), "Administration", new { courseId });
        }

        public IActionResult Edit(string lectureId)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            var formModel = lectureService.GetLectureById<EditLectureFormModel>(lectureId);

            if (formModel == null) return NotFound();

            formModel.Descriptions = lectureService.GetLectureDescriptions<EditDescriptionFormModel>(lectureId);

            return View(formModel);
        }

        [HttpPost]
        public IActionResult Edit(string lectureId,EditLectureFormModel lecture)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            lectureService.EditLectureById(lectureId,lecture);

            return RedirectToAction(nameof(AdministrationController.Lectures), "Administration", new { courseId = lecture.CourseId });
        }
    }
}