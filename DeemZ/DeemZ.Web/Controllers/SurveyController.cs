namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.SurveyServices;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.ViewModels.Surveys;
    using Microsoft.AspNetCore.Http;
    using DeemZ.Web.Infrastructure;

    public class SurveyController : Controller
    {
        private readonly Guard guard;
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService, Guard guard)
        {
            this.surveyService = surveyService;
            this.guard = guard;
        }

        [Authorize]
        public IActionResult Take(string surveyId)
        {
            if (guard.AgainstNull(surveyId, nameof(surveyId))) return NotFound();

            var userId = User.GetId();

            if (CheckIfUserHavePermissionToThisSurvey(userId,surveyId)) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            return View(survey);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Take(IFormCollection answers)
        {
            if (guard.AgainstNull(answers)) return NotFound();

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
