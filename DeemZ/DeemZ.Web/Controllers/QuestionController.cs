namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services.Question;
    using DeemZ.Services.ExamServices;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize(Roles = AdminRoleName)]
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IExamService examService;

        public QuestionController(IQuestionService questionService, IExamService examService)
        {
            this.questionService = questionService;
            this.examService = examService;
        }

        [ClientRequired]
        public IActionResult Add(string examId)
        {
            if (!examService.GetExamById(examId)) return NotFound();

            return View();
        }

        [HttpPost]
        [ClientRequired]
        public IActionResult Add(string examId, AddQuestionFormModel question)
        {
            if (!examService.GetExamById(examId)) return NotFound();

            var error = questionService.ValidateQuestionFormModel(question);

            if (error != null ) ModelState.AddModelError(nameof(question.Answers), error);

            if (!ModelState.IsValid) return View(question);

            questionService.AddQuestionToExam(examId, question);

            return RedirectToAction(nameof(AdministrationController.Questions), typeof(AdministrationController).GetControllerName(), new { examId, area = AreaNames.AdminArea });
        }

        [ClientRequired]
        public IActionResult Delete(string questionId)
        {
            if (!questionService.GetQuestionById(questionId)) return NotFound();

            var examId = questionService.Delete(questionId);

            return RedirectToAction(nameof(AdministrationController.Questions), typeof(AdministrationController).GetControllerName(), new { examId, area = AreaNames.AdminArea });
        }

        [ClientRequired]
        public IActionResult Edit(string questionId)
        {
            if (!questionService.GetQuestionById(questionId)) return NotFound();

            var viewModel = questionService.GetQuestionById<AddQuestionFormModel>(questionId);

            int maxQuestionCount = 4;
            int diff = maxQuestionCount - viewModel.Answers.Count;


            for (int i = 0; i < diff; i++)
            {
                viewModel.Answers.Add(null);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ClientRequired]
        public IActionResult Edit(string questionId, AddQuestionFormModel question)
        {
            if (!questionService.GetQuestionById(questionId)) return NotFound();

            var error = questionService.ValidateQuestionFormModel(question);

            if (error != null) ModelState.AddModelError(nameof(question.Answers), error);

            if (!ModelState.IsValid) return View(question);

            string examId = questionService.Edit(questionId, question);

            return RedirectToAction(nameof(AdministrationController.Questions), typeof(AdministrationController).GetControllerName(), new { examId, area = AreaNames.AdminArea });
        }
    }
}