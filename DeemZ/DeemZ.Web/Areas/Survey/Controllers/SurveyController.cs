namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Services.CourseServices;
    using DeemZ.Data.Models;

    [Area(AreaName.SurveyArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class SurveyController : BaseController
    {
        private readonly ISurveyService surveyService;
        private readonly ICourseService courseService;

        public SurveyController(ISurveyService surveyService, ICourseService courseService)
        {
            this.surveyService = surveyService;
            this.courseService = courseService;
        }

        [ClientRequired]
        [IfExists]
        public IActionResult All(string courseId)
        {
            var allSurveys = surveyService.GetSurveysByCourseId<DetailsSurveyViewModel>(courseId);

            ViewBag.CourseId = courseId;

            return View(allSurveys);
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Add(string courseId)
        {
            var courseName = courseService.GetCourseById<Course>(courseId).Name;

            var model = new AddSurveyFormModel()
            {
                CourseName = courseName
            };

            return View(model);
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Add(string courseId, AddSurveyFormModel survey)
        {
            if (!ModelState.IsValid)
            {
                survey.CourseName = courseService.GetCourseById<Course>(courseId).Name;
                
                return View(survey);
            }

            surveyService.AddSurveyToCourse(courseId, survey);

            return RedirectToAction(nameof(All), new { courseId });
        }
    }
}