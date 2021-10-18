namespace DeemZ.Web.Areas.Survey.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Web.Controllers;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Models.FormModels.Survey;

    [Area(AreaName.SurveyArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class QuestionController : BaseController
    {
        private readonly ISurveyService surveyService;

        public QuestionController(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        [ClientRequired]
        [IfExists]
        public IActionResult All(string surveyId)
        {
            var questions = surveyService.GetSurveyQuestions<AllSurveyQuestionsViewModel>(surveyId);

            ViewBag.SurveyId = surveyId;

            return View(questions);
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Add(string surveyId)
            => View();

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Add(string surveyId, AddSurveyQuestionFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            surveyService.AddQuestionToSurvey(surveyId, model);

            return RedirectToAction(nameof(All), new { surveyId });
        }
    }
}
