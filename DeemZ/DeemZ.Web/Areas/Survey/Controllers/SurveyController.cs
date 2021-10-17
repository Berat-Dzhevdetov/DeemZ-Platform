namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.SurveyArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class SurveyController : BaseController
    {
        private readonly ISurveyService surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        [ClientRequired]
        [IfExists]
        public IActionResult All(string courseId)
        {
            var allSurveys = surveyService.GetSurveysByCourseId<DetailsSurveyViewModel>(courseId);

            ViewBag.CourseId = courseId;

            return View(allSurveys);
        }
    }
}