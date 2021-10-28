namespace DeemZ.Web.Areas.Survey.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Web.Filters;
    using DeemZ.Data.Models;
    using DeemZ.Web.Controllers;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.SurveyArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class AnswerController : BaseController
    {
        private readonly ISurveyService surveyService;

        public AnswerController(ISurveyService surveyService) => this.surveyService = surveyService;

        [ClientRequired]
        [IfExists]
        public IActionResult All(string surveyQuestionId)
        {
            var question = surveyService.GetQuestionById<SurveyQuestion>(surveyQuestionId);

            if (question.IsOpenAnswer)
                return HandleErrorRedirect(Models.HttpStatusCodes.NotFound);

            var answers = surveyService.GetAllAnswers<AllSurveyQuestionAnswersViewModel>(surveyQuestionId);

            ViewBag.QuestionId = question.Id;

            return View(answers);
        }
    }
}
