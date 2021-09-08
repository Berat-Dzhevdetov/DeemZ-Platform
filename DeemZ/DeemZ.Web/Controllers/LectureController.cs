namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services;
    using DeemZ.Services.LectureServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Models.FormModels.Description;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize(Roles = AdminRoleName)]
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

        [ClientRequired]
        public IActionResult Add(string courseId)
        {
            if (!courseService.GetCourseById(courseId)) return NotFound();

            return View();
        }

        [HttpPost]
        [ClientRequired]
        public IActionResult Add(string courseId, AddLectureFormModel lecture)
        {
            if (!courseService.GetCourseById(courseId)) return NotFound();

            if (!ModelState.IsValid) return View(lecture);

            lectureService.AddLectureToCourse(courseId, lecture);

            return RedirectToAction(nameof(AdministrationController.Lectures), typeof(AdministrationController).GetControllerName(), new { courseId, area = AreaNames.AdminArea });
        }

        [ClientRequired]
        public IActionResult Edit(string lectureId)
        {
            var formModel = lectureService.GetLectureById<EditLectureFormModel>(lectureId);

            if (formModel == null) return NotFound();

            formModel.Descriptions = lectureService.GetLectureDescriptions<EditDescriptionFormModel>(lectureId);

            return View(formModel);
        }

        [HttpPost]
        [ClientRequired]
        public IActionResult Edit(string lectureId, EditLectureFormModel lecture)
        {
            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            lectureService.EditLectureById(lectureId, lecture);

            return RedirectToAction(nameof(AdministrationController.Lectures), typeof(AdministrationController).GetControllerName(), new { courseId = lecture.CourseId, area = AreaNames.AdminArea });
        }

        [Authorize(Roles = AdminRoleName)]
        [ClientRequired]
        public IActionResult Delete(string lectureId)
        {
            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            lectureService.DeleteLecture(lectureId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName());
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public ActionResult DeleteDescription(string did)
        {
            if (guard.AgainstNull(did, nameof(did))) return Json(new { status = 400, message = "The given id was null" });
            lectureService.DeleteDescription(did);
            return Json(new { status = 200, message = "The description was successfully deleted" });
        }
    }
}