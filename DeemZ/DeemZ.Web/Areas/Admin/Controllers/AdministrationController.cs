namespace DeemZ.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using DeemZ.Services.AdminServices;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.ViewModels.Resources;
    using DeemZ.Services.LectureServices;
    using DeemZ.Models.ViewModels.Lectures;
    using DeemZ.Services.UserServices;
    using DeemZ.Models.ViewModels.User;
    using DeemZ.Services.ReportService;
    using DeemZ.Models.ViewModels.Reports;
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Services.ExamServices;
    using DeemZ.Services.Question;
    using DeemZ.Global.Extensions;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize(Roles = AdminRoleName)]
    [Area(AreaNames.AdminArea)]
    public class AdministrationController : Controller
    {
        private readonly IAdminService adminService;
        private readonly ICourseService courseService;
        private readonly ILectureService lectureService;
        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly IExamService examService;
        private readonly IQuestionService questionService;

        public AdministrationController(IAdminService adminService, ICourseService courseService, ILectureService lectureService, IUserService userService, IReportService reportService, IExamService examService, IQuestionService questionService)
        {
            this.adminService = adminService;
            this.courseService = courseService;
            this.lectureService = lectureService;
            this.userService = userService;
            this.reportService = reportService;
            this.examService = examService;
            this.questionService = questionService;
        }

        public IActionResult Index(int page = 1, int quantity = 20)
        {
            var viewModel = adminService.GetIndexPageInfo();

            var allPages = (int)Math.Ceiling(adminService.GetUserCoursesCount() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel.UserCourses = adminService.GetUserCourses<UserCoursesViewModel>(page, quantity);

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        public IActionResult Courses(int page = 1, int quantity = 20)
        {
            var viewModel = new AdministrationCoursesViewModel();

            var allPages = (int)Math.Ceiling(adminService.GetUserCoursesCount() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel.Courses = (List<CoursesViewModel>)adminService.GetAllCourses<CoursesViewModel>(page, quantity);

            foreach (var course in viewModel.Courses)
            {
                course.SignedUpUsers = adminService.GetUserSignedUpForCourse(course.Id);
            }

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        [ClientRequired("lectureId")]
        public IActionResult Resources(string lectureId, int page = 1, int quantity = 20)
        {
            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            var resources = lectureService.GetLectureResourcesById<IndexResourceViewModel>(lectureId);

            var allPages = (int)Math.Ceiling(resources.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            var viewModel = new ResourcesForCourseViewModel();

            viewModel.Recourses = resources.Paging(page, quantity).ToList();

            viewModel = AdjustPages(viewModel, page, allPages);

            viewModel.LectureId = lectureId;

            return View(viewModel);
        }

        [ClientRequired("courseId")]
        public IActionResult Lectures(string courseId, int page = 1, int quantity = 20)
        {
            if (!courseService.GetCourseById(courseId)) return NotFound();

            var lectures = lectureService.GetLecturesByCourseId<LectureBasicInformationViewModel>(courseId);

            var allPages = (int)Math.Ceiling(lectures.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            var viewModel = new IndexLecturesViewModel();

            viewModel.CourseId = courseId;

            viewModel.Lectures = lectures.Paging(page, quantity).ToList();

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        public IActionResult Users(int page = 1, int quantity = 20)
        {
            var viewModel = new AdmistrationUsersViewModel();

            var users = userService.GetAllUsers<BasicUserInformationViewModel>(page, quantity);

            var allPages = (int)Math.Ceiling(users.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel.Users = users;

            viewModel = AdjustPages(viewModel, page, allPages);

            foreach (var user in viewModel.Users)
            {
                user.TakenCoursesCount = userService.GetUserTakenCourses(user.Id);
            }

            return View(viewModel);
        }

        public IActionResult Reports(int page = 1, int quantity = 20)
        {
            var viewModel = new AdministrationReportViewModel();

            viewModel.Reports = reportService.GetReports<ReportViewReport>(page, quantity);

            var allPages = (int)Math.Ceiling(viewModel.Reports.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        [ClientRequired]
        public IActionResult Exams(string courseId)
        {
            if (!courseService.GetCourseById(courseId)) return NotFound();

            var viewModel = new AdministrationExamsViewModel();

            viewModel.CourseId = courseId;

            viewModel.Exams = examService.GetExamsByCourseId<BasicExamInfoViewModel>(courseId);

            return View(viewModel);
        }

        [ClientRequired]
        public IActionResult Questions(string examId)
        {
            if (!examService.GetExamById(examId)) return NotFound();

            var viewModel = new AdministrationQuetionsViewModel();

            viewModel.ExamId = examId;

            viewModel.Questions = questionService.GetQuestionsByExamId<QuetionsViewModel>(examId);

            return View(viewModel);
        }

        public IActionResult UserCourses(int page = 1,int quantity = 20)
        {
            var viewModel = new AdministrationUserCoursesViewModel();

            viewModel.UserCourses = courseService.GetUserCourses<UserCoursesViewModel>(page, quantity);

            var allPages = (int)Math.Ceiling(viewModel.UserCourses.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
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

        public IActionResult Chat(string groupId = "admin")
        {
            ViewBag.GroupId = groupId;
            ViewBag.UserId = User.GetId();
            return View();
        }
    }
}