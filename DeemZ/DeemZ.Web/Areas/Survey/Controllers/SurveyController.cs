﻿namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Web.Filters;
    using DeemZ.Services.SurveyServices;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Services.CourseServices;
    using DeemZ.Data.Models;
    using DeemZ.Web.Infrastructure;

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
    }
}