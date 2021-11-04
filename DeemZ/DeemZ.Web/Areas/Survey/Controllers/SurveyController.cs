namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Services.CourseServices;
    using DeemZ.Data.Models;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.Shared;

    using static DeemZ.Global.WebConstants.Constant;

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
            var model = new AddSurveyFormModel()
            {
                CourseName = courseService.GetCourseById<Course>(courseId).Name
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

        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string surveyId)
        {
            var survey = surveyService.GetSurveyById<EditSurveyFormModel>(surveyId);

            survey.StartDate = survey.StartDate.ToLocalTime();
            survey.EndDate = survey.EndDate.ToLocalTime();

            return View(survey);
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Edit(string surveyId, EditSurveyFormModel survey)
        {
            if (!ModelState.IsValid)
                return View(survey);

            var courseId = surveyService.EditSurvey(surveyId, survey);

            return RedirectToAction(nameof(All), new { courseId });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string surveyId)
        {
            string courseId = surveyService.DeleteSurvey(surveyId);

            return RedirectToAction(nameof(All), new { courseId });
        }

        [ClientRequired]
        [IfExists]
        [Authorize]
        public IActionResult Take(string surveyId)
        {
            var isAdmin = User.IsAdmin();

            if (!isAdmin)
            {
                var userId = User.GetId();

                var canUserAccessThisSurvey = surveyService.CanUserAccessSurveyById(surveyId, userId);

                if (!canUserAccessThisSurvey)
                    return HandleErrorRedirect(Models.HttpStatusCodes.Forbidden);
            }

            surveyService.PrepareSurvey(surveyId, out TakeSurveyFormModel survey);

            return View(survey);
        }

        [ClientRequired]
        [IfExists]
        [Authorize]
        [HttpPost]
        public IActionResult Take(string surveyId, TakeSurveyFormModel survey)
        {
            var (errors, correctAnswerIds) = surveyService.ValidateSurvey(survey);

            foreach (var error in errors)
                ModelState.AddModelError(error.Key, error.Value);

            if (!ModelState.IsValid)
            {
                surveyService.PrepareSurvey(surveyId, out survey);
                return View(survey);
            }

            var userId = User.GetId();

            surveyService.SaveSurvey(surveyId, userId, correctAnswerIds);

            TempData[GlobalMessageKey] = "Thank you very much for your feedback. It helps us grow!";

            return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName(), new { area = "" });
        }

        [Authorize]
        public IActionResult MySurveys(int page = 1)
        {
            var model = new MySurveyPagingViewModel();

            var userId = User.GetId();

            var allPages = (int)Math.Ceiling(surveyService.GetUserAllSurveysCount(userId) / (25 * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            model = AdjustPages(model, page, allPages);

            model.Surveys = surveyService.GetUserAllSurveys<MySurveyViewModel>(userId, page);

            return View(model);
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public IActionResult Preview(string surveyId)
        {
            var isAdmin = User.IsAdmin();

            var userId = User.GetId();
            if (!isAdmin)
            {
                var doesTheUserHavePermissionToSurvey = surveyService.DoesTheUserHavePermissionToSurvey(userId, surveyId);

                if (!doesTheUserHavePermissionToSurvey)
                    HandleErrorRedirect(Models.HttpStatusCodes.Forbidden);
            }

            var surveys = surveyService.GetSurveyById<MySurveyViewModel>(surveyId);

            surveys.UserAnswers = surveyService.GetUserAnswers(userId, surveyId);

            return View(surveys);
        }

        private T AdjustPages<T>(T viewModel, int page, int allPages)
            where T : PagingBaseModel
        {
            viewModel.CurrentPage = page;
            viewModel.NextPage = page >= allPages ? null : page + 1;
            viewModel.PreviousPage = page <= 1 ? null : page - 1;
            viewModel.MaxPages = allPages;

            return viewModel;
        }
    }
}