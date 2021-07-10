namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.SurveyServices;

    public class SurveyController : Controller
    {
        private readonly Guard guard;
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService, Guard guard)
        {
            this.surveyService = surveyService;
            this.guard = guard;
        }

        public IActionResult Take(string surveyId)
        {
            if(guard.AgainstNull(surveyId,nameof(surveyId))) return NotFound();



            return View();
        }
    }
}
