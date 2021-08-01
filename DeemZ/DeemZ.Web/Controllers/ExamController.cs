namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.ExamServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Exam;

    using static Constants;

    [Authorize]
    public class ExamController : Controller
    {
        private readonly Guard guard;
        private readonly IExamService examService;
        private readonly ICourseService courseService;

        public ExamController(Guard guard, IExamService examService, ICourseService courseService)
        {
            this.guard = guard;
            this.examService = examService;
            this.courseService = courseService;
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add(string courseId, AddExamFormModel exam)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            if (!ModelState.IsValid) return View(exam);

            examService.CreateExam(courseId, exam);

            return RedirectToAction(nameof(AdministrationController.Exams), "Administration", new { courseId });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string examId)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            var exam = examService.GetExamById<AddExamFormModel>(examId);

            return View(exam);
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string examId, AddExamFormModel exam)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            if (!ModelState.IsValid) return View(exam);

            var courseId = examService.EditExam(examId, exam);

            return RedirectToAction(nameof(AdministrationController.Exams), "Administration", new { courseId });
        }
    }
}