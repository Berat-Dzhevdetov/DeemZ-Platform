namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using DeemZ.Services;
    using DeemZ.Services.ExamServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Services.UserServices;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize]
    public class ExamController : Controller
    {
        private readonly Guard guard;
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private readonly IUserService userService;

        public ExamController(Guard guard, IExamService examService, ICourseService courseService, IUserService userService)
        {
            this.guard = guard;
            this.examService = examService;
            this.courseService = courseService;
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Take(string examId, string password)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            var userId = User.GetId();

            var isUserAdmin = await userService.IsInRole(userId, AdminRoleName);

            if (!isUserAdmin)
            {
                if (!examService.DoesTheUserHavePermissionToExam(userId, examId)) return Unauthorized();

                ViewBag.ExamId = examId;

                if (guard.AgainstNull(password, nameof(password))) return View("Password");
            }

            var exam = examService.GetExamById<BasicExamInfoViewModel>(examId);

            if (password != exam.Password && !isUserAdmin)
            {
                ViewBag.PasswordValidation = "Wrong password";
                return View("Password");
            }

            return View(exam);
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