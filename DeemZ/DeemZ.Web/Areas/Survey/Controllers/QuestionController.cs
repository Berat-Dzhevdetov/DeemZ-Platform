namespace DeemZ.Web.Areas.Survey.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using DeemZ.Web.Controllers;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.FormModels.Survey;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.SurveyArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class QuestionController : BaseController
    {
        private readonly ISurveyService surveyService;

        public QuestionController(ISurveyService surveyService) => this.surveyService = surveyService;

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

        public ISurveyService GetSurveyService()
        {
            return surveyService;
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public async Task<IActionResult> AddAsync(string surveyId, AddSurveyQuestionFormModel model, ISurveyService surveyService)
        {
            if (!ModelState.IsValid)
                return View(model);

            var questionId = await surveyService.AddQuestionToSurvey(surveyId, model);

            if(model.CreateRatingScale)
            {
                await surveyService.AddRatingScaleToQuestion(questionId);
            }

            return RedirectToAction(nameof(All), new { surveyId });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string surveyQuestionId)
        {
            var question = surveyService.GetQuestionById<EditSurveyQuestionFormModel>(surveyQuestionId);
            return View(question);
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Edit(string surveyQuestionId, EditSurveyQuestionFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var surveyId = surveyService.EditQuestion(surveyQuestionId, model);

            return RedirectToAction(nameof(All), new { surveyId });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string surveyQuestionId)
        {
            var surveyId = surveyService.DeleteQuestion(surveyQuestionId);

            return RedirectToAction(nameof(All), new { surveyId });
        }
    }
}
