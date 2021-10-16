namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.ReportArea)]
    public class SurveyController : BaseController
    {
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public IActionResult All(string courseId)
        {
            //var allSurveys = surveyService.get
            return View();
        }
    }
}