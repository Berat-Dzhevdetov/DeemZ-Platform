namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.SurveyServices;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class SurveyController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Guard guard;
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService, Guard guard, UserManager<ApplicationUser> userManager)
        {
            this.surveyService = surveyService;
            this.guard = guard;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> TakeAsync(string surveyId)
        {
            if(guard.AgainstNull(surveyId,nameof(surveyId))) return NotFound();

            if (!await CheckIfUserHavePermissionToThisSurveyAsync(surveyId)) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            return View(survey);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TakeAsync(IFormCollection answers)
        {
            if (guard.AgainstNull(answers)) return NotFound();

            var surveyId = answers["surveyId"].ToString();

            if (!await CheckIfUserHavePermissionToThisSurveyAsync(surveyId)) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            var questionsIds = answers["questionId"];

            foreach (var questionId in questionsIds)
            {

            }

            return View();
        }

        private async Task<bool> CheckIfUserHavePermissionToThisSurveyAsync(string surveyId)
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var doesThisUserHavePermissionToTakeTheSurvey = surveyService.DoesTheUserHavePermissionToSurvey(user.Id, surveyId);

            if (!doesThisUserHavePermissionToTakeTheSurvey) return false;
            return true;
        }
    }
}
