namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.SurveyServices;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.ViewModels.Surveys;
    using Microsoft.AspNetCore.Http;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Filters;

    public class SurveyController : Controller
    {
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService) => this.surveyService = surveyService;

        [Authorize]
        [ClientRequired]
        [IfExists]
        public IActionResult Take(string surveyId)
        {
            var userId = User.GetId();

            if (CheckIfUserHavePermissionToThisSurvey(userId,surveyId)) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            return View(survey);
        }

        [Authorize]
        [HttpPost]
        [ClientRequired]
        public IActionResult Take(IFormCollection answers)
        {
            var surveyId = answers["surveyId"].ToString();

            var userId = User.GetId();

            if (CheckIfUserHavePermissionToThisSurvey(userId, surveyId)) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            var questionsIds = answers["questionId"];

            foreach (var questionId in questionsIds)
            {
                //todo:
            }

            return View();
        }

        private bool CheckIfUserHavePermissionToThisSurvey(string uid, string surveyId)
        {
            return surveyService.DoesTheUserHavePermissionToSurvey(uid, surveyId);
        }
    }
}
