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

            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var doesThisUserHavePermissionToTakeTheSurvey = surveyService.DoesThisUserHavePermissionToTakeTheSurvey(user.Id,surveyId);

            if (!doesThisUserHavePermissionToTakeTheSurvey) return Unauthorized();

            var survey = surveyService.GetSurveyById<TakeSurveyViewModel>(surveyId);

            if (survey == null) return NotFound();

            return View(survey);
        }
    }
}
