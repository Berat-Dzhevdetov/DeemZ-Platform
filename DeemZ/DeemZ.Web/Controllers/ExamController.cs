﻿namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System;
    using DeemZ.Services;
    using DeemZ.Services.ExamServices;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.UserServices;
    using DeemZ.Global.Extensions;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Infrastructure;
    using DeemZ.Web.Filters;
    using DeemZ.Models;
    using DeemZ.Services.PdfServices;

    using static DeemZ.Global.WebConstants.Constant;
    using static DeemZ.Global.WebConstants.UserErrorMessages;

    [Authorize]
    public class ExamController : BaseController
    {
        private readonly Guard guard;
        private readonly IExamService examService;
        private readonly ICourseService courseService;
        private readonly IUserService userService;
        private readonly IPdfService pdfService;

        private const string IsPasswordProvidedKey = "IsPasswordProvided";
        private const string PassingTheTest = "PassingTheTestKey";
        private const string PasswordIsRequired = "Password is required";
        private const string WrongPassword = "Wrong password";
        private const string MessageAfterExam = "Congratulations, you have earned {0}/{1} points";
        private const string MessageAfterExamWithCertificate = $"{MessageAfterExam} and you can also see your certificate in your profile.";
        private const string LateHandOverExam = "Sorry, but you hand over the exam too late and you officially receive 0 points";

        public ExamController(Guard guard, IExamService examService, ICourseService courseService, IUserService userService, IPdfService pdfService)
        {
            this.guard = guard;
            this.examService = examService;
            this.courseService = courseService;
            this.userService = userService;
            this.pdfService = pdfService;
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Access(string examId)
        {
            var userId = User.GetId();

            var isUserAdmin = User.IsAdmin();

            if (isUserAdmin)
            {
                TempData[IsPasswordProvidedKey] = true;
                return RedirectToAction(nameof(ExamController.Take), new { examId });
            }

            if (!isUserAdmin && !examService.DoesTheUserHavePermissionToExam(userId, examId))
                return RedirectToAction(nameof(BaseController.HandleErrorRedirect), typeof(BaseController).GetControllerName(), new { errorCode = HttpStatusCodes.BadRequest });

            return View();
        }

        [ClientRequired("examId")]
        [IfExists]
        [HttpPost]
        public IActionResult Access(string examId, string password)
        {
            if (guard.AgainstNull(password, nameof(password)))
            {
                ViewBag.PasswordValidation = PasswordIsRequired;
                return View();
            }

            var userId = User.GetId();

            if (!examService.DoesTheUserHavePermissionToExam(userId, examId))
                return HandleErrorRedirect(HttpStatusCodes.Forbidden);

            var isProvidedPasswordRight = examService.IsProvidedPasswordRight(examId, password);

            if (!isProvidedPasswordRight)
            {
                ViewBag.PasswordValidation = WrongPassword;
                return View();
            }

            TempData[IsPasswordProvidedKey] = true;

            return RedirectToAction(nameof(ExamController.Take), new { examId });
        }

        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> Take(string examId)
        {
            var userId = User.GetId();

            if (!examService.DoesTheUserHavePermissionToExam(userId, examId))
                return HandleErrorRedirect(HttpStatusCodes.Forbidden);

            bool isPasswordProvided = TempData[IsPasswordProvidedKey] is bool;

            if (!isPasswordProvided) return RedirectToAction(nameof(ExamController.Access), new { examId });

            var exam = examService.GetExamById<TakeExamFormModel>(examId);

            exam.StartDate = exam.StartDate.ToLocalTime();
            exam.EndDate = exam.EndDate.ToLocalTime();

            var isUserAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

            if (!exam.IsPublic && !isUserAdmin)
                return RedirectToAction(nameof(BaseController.HandleErrorRedirect), typeof(BaseController).GetControllerName(), new { errorCode = HttpStatusCodes.Forbidden });

            if (exam.ShuffleQuestions) exam.Questions.Shuffle();

            if (exam.ShuffleAnswers) exam.Questions.ForEach(x => x.Answers.Shuffle());

            TempData[PassingTheTest] = true;

            ViewData["LocalRecaptchaSiteKey"] = Secret.GoogleRecaptcha.LocalRecaptchaSiteKey;

            return View(exam);
        }

        [HttpPost]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> Take(string examId, TakeExamFormModel exam)
        {
            var userId = User.GetId();

            if (!examService.DoesTheUserHavePermissionToExam(userId, examId))
                return HandleErrorRedirect(HttpStatusCodes.Forbidden);

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(HomeController.UserErrorPage), typeof(HomeController).GetControllerName(), new { InvalidForm });

            bool passingTheTest = TempData[PassingTheTest] is bool;

            if (!passingTheTest) return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());

            var isUserAdmin = User.IsAdmin() || User.IsLecture();

            if (!exam.IsPublic && !isUserAdmin)
                return RedirectToAction(nameof(BaseController.HandleErrorRedirect), typeof(BaseController).GetControllerName(), new { errorCode = HttpStatusCodes.Forbidden });

            var points = await examService.EvaluateExam(exam, userId);

            var maxPoints = await examService.SaveUserExam(userId, points, examId);

            if (maxPoints == -1)
            {
                TempData[GlobalMessageKey] = LateHandOverExam;
                return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
            }

            var message = string.Empty;

            if (points >= 0 || maxPoints > 0)
            {
                var grade = Math.Round(points / (maxPoints * 1.0)) * 100;
                if (grade >= 80)
                {
                    var serverLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    var courseId = examService.GetCourseIdByExamId(examId);
                    await pdfService.GenerateCertificate(grade, courseId, userId, serverLink);
                    message = MessageAfterExamWithCertificate;
                }
                else
                {
                    message = MessageAfterExam;
                }
            }

            TempData[GlobalMessageKey] = string.Format(message, points, maxPoints);
            
            return RedirectToAction(nameof(GetUserExams));
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Add(string courseId) => View();

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Add(string courseId, AddExamFormModel exam)
        {
            if (!ModelState.IsValid) return View(exam);

            examService.CreateExam(courseId, exam);

            return RedirectToAction(nameof(AdministrationController.Exams), typeof(AdministrationController).GetControllerName(), new { courseId, area = AreaName.AdminArea });
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string examId)
        {
            var exam = examService.GetExamById<AddExamFormModel>(examId);

            exam.StartDate = exam.StartDate.ToLocalTime();
            exam.EndDate = exam.EndDate.ToLocalTime();

            return View(exam);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string examId, AddExamFormModel exam)
        {
            if (!ModelState.IsValid) return View(exam);

            examService.EditExam(examId, exam);

            string courseId = examService.GetCourseIdByExamId(examId);

            return RedirectToAction(nameof(AdministrationController.Exams), typeof(AdministrationController).GetControllerName(), new { courseId, area = AreaName.AdminArea });
        }

        public IActionResult GetUserExams()
        {
            var userId = User.GetId();

            var exams = examService.GetExamsByUserId<GetUserExamInfoViewModel>(userId);

            foreach (var exam in exams)
                exam.EndDate = exam.EndDate.ToLocalTime();

            return View(exams);
        }

        [ClientRequired]
        [IfExists]
        public IActionResult ViewExam(string examId)
        {
            var userId = User.GetId();

            var isUserInRole = User.IsAdmin() || User.IsLecture();

            if (!isUserInRole && !examService.DoesTheUserHavePermissionToExam(userId, examId))
                return HandleErrorRedirect(HttpStatusCodes.Forbidden);

            var exam = examService.GetExamById<ViewExamViewModel>(examId);

            exam.EndDate = exam.EndDate.ToLocalTime();

            if (DateTime.Now <= exam.EndDate && !isUserInRole)
                return HandleErrorRedirect(HttpStatusCodes.Forbidden);

            exam.UserAnswers = examService.GetUserExamAnswers(examId, userId);

            return View(exam);
        }
    }
}