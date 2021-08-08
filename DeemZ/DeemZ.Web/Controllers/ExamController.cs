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
    using DeemZ.Services.UserServices;
    using DeemZ.Global.Extensions;
    using DeemZ.Web.Areas.Administration.Controllers;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize]
    public class ExamController : Controller
    {
        private readonly Guard guard;
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private readonly IUserService userService;

        private const string IsPasswordProvidedKey = "IsPasswordProvided";
        private const string PassingTheTest = "PassingTheTestKey";
        private const string PasswordIsRequired = "Password is required";
        private const string WrongPassword = "Wrong password";
        private const string MessageAfterExam = "Congratulations, you have earned {0}/{1} points";
        private const string LateHandOverExam = "Sorry, but you hand over the exam too late and you officially receive 0 points";

        public ExamController(Guard guard, IExamService examService, ICourseService courseService, IUserService userService)
        {
            this.guard = guard;
            this.examService = examService;
            this.courseService = courseService;
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Access(string examId)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            var userId = User.GetId();

            var isUserAdmin = await userService.IsInRole(userId, AdminRoleName);

            if (isUserAdmin)
            {
                TempData[IsPasswordProvidedKey] = true;
                return RedirectToAction(nameof(ExamController.Take), new { examId });
            }

            if (!isUserAdmin && !examService.DoesTheUserHavePermissionToExam(userId, examId)) return Unauthorized();

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Access(string examId, string password)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            if (guard.AgainstNull(password, nameof(password)))
            {
                ViewBag.PasswordValidation = PasswordIsRequired;
                return View();
            }

            var userId = User.GetId();

            if (!examService.DoesTheUserHavePermissionToExam(userId, examId)) return Unauthorized();

            var isProvidedPasswordRight = examService.IsProvidedPasswordRight(examId, password);

            if (!isProvidedPasswordRight)
            {
                ViewBag.PasswordValidation = WrongPassword;
                return View();
            }

            TempData[IsPasswordProvidedKey] = true;

            return RedirectToAction(nameof(ExamController.Take), new { examId });
        }

        [Authorize]
        public async Task<IActionResult> Take(string examId)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            bool isPasswordProvided = TempData[IsPasswordProvidedKey] is bool;

            if (!isPasswordProvided) return RedirectToAction(nameof(ExamController.Access), new { examId });

            var exam = examService.GetExamById<TakeExamFormModel>(examId);

            exam.StartDate = exam.StartDate.ToLocalTime();
            exam.EndDate = exam.EndDate.ToLocalTime();

            var userId = User.GetId();

            var isUserAdmin = await userService.IsInRole(userId, AdminRoleName);

            if (!exam.IsPublic && !isUserAdmin) return BadRequest();

            if (exam.ShuffleQuestions) exam.Questions.Shuffle();

            if (exam.ShuffleAnswers) exam.Questions.ForEach(x => x.Answers.Shuffle());

            TempData[PassingTheTest] = true;

            return View(exam);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Take(string examId, TakeExamFormModel exam)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            bool passingTheTest = TempData[PassingTheTest] is bool;

            if (!passingTheTest) return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());

            var userId = User.GetId();

            var isUserAdmin = await userService.IsInRole(userId, AdminRoleName);

            if (!exam.IsPublic && !isUserAdmin) return BadRequest();

            var points = examService.EvaluateExam(exam);

            var maxPoints = examService.SaveUserExam(userId, points, examId);

            if (maxPoints == -1)
            {
                TempData[GlobalMessageKey] = LateHandOverExam;
                return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
            }

            TempData[GlobalMessageKey] = string.Format(MessageAfterExam, points, maxPoints);

            return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
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

            return RedirectToAction(nameof(AdministrationController.Exams), typeof(AdministrationController).GetControllerName(), new { courseId });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string examId)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            var exam = examService.GetExamById<AddExamFormModel>(examId);

            exam.StartDate = exam.StartDate.ToLocalTime();
            exam.EndDate = exam.EndDate.ToLocalTime();

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

            return RedirectToAction(nameof(AdministrationController.Exams), typeof(AdministrationController).GetControllerName(), new { courseId });
        }
    }
}