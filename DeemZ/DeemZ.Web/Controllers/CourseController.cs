namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services.CourseServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.LectureServices;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Services.UserServices;
    using DeemZ.Web.Filters;
    using DeemZ.Models;

    using static Global.WebConstants.Constant;

    public class CourseController : BaseController
    {
        private readonly ICourseService courseService;
        private readonly ILectureService lectureService;
        private readonly IUserService userService;

        public CourseController(ICourseService courseService, ILectureService lectureService, IUserService userService)
        {
            this.courseService = courseService;
            this.lectureService = lectureService;
            this.userService = userService;
        }

        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> ViewCourse(string courseId)
        {
            var course = courseService.GetCourseById<DetailsCourseViewModel>(courseId);

            string userId = null;

            if (User.Identity.IsAuthenticated) userId = User.GetId();

            if (userId == null) course.IsUserSignUpForThisCourse = false;
            else if (await userService.IsInRoleAsync(userId, Role.AdminRoleName)) course.IsUserSignUpForThisCourse = true;
            else course.IsUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            return View(course);
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> SignUp(string courseId)
        {
            var userId = User.GetId();

            if (await userService.IsInRoleAsync(userId, Role.AdminRoleName)) return Unauthorized();

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            return View(course);
        }

        [Authorize]
        [HttpPost]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> SignUp(string courseId, SignUpCourseFormModel signUp)
        {
            var userId = User.GetId();

            if (await userService.IsInRoleAsync(userId, Role.AdminRoleName)) return Unauthorized();

            var isUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            if (isUserSignUpForThisCourse) return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });

            if (!ModelState.IsValid) return View(signUp);

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            courseService.SignUserToCourse(userId, course.Id);

            return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });
        }

        public IActionResult UpcomingCourses()
        {
            var viewModel = new UpcomingCoursesViewModel
            {
                UpcomingCourses = courseService.GetCoursesForSignUp<UpcomingCourseViewModel>(),
            };
            return View(viewModel);
        }

        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Add() => View();


        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        public IActionResult Add(AddCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            var courseId = courseService.CreateCourse(course);

            if (course.BasicLectures) courseService.CreateBasicsLectures(courseId, course);

            if (course.Redirect) return RedirectToAction(nameof(CourseController.ViewCourse), typeof(CourseController).GetControllerName(), new { courseId });

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string courseId)
        {
            var course = courseService.GetCourseById<EditCourseFormModel>(courseId);

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string courseId, EditCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            courseService.Edit(course, courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string courseId)
        {
            courseService.DeleteCourse(courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult AddUserToCourse()
        {
            var model = new AddUserToCourseFormModel();

            var prevDate = DateTime.Now.AddDays(-14);

            model.Courses = courseService.GetCourseByIdAsKeyValuePair(prevDate);

            return View(model);
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [HttpPost]
        [ClientRequired]
        public IActionResult AddUserToCourse(AddUserToCourseFormModel model)
        {
            if (!courseService.GetCourseById(model.CourseId)) ModelState.AddModelError(nameof(AddUserToCourseFormModel.CourseId), "Invalid course!");

            if (!userService.GetUserByUserName(model.Username)) ModelState.AddModelError(nameof(AddUserToCourseFormModel.Username), "User not found!");

            if (!ModelState.IsValid)
            {
                var prevDate = DateTime.Now.AddDays(-14);
                model.Courses = courseService.GetCourseByIdAsKeyValuePair(prevDate);
                return View(model);
            }

            var userId = userService.GetUserIdByUserName(model.Username);

            courseService.SignUserToCourse(userId, model.CourseId, model.IsPaid);

            return RedirectToAction(nameof(AdministrationController.UserCourses),typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists("courseId")]
        public IActionResult DeleteUserFromCourse(string courseId, string userId)
        {
            if(!userService.GetUserById(userId)) return RedirectToAction(nameof(BaseController.HandleErrorRedirect), typeof(BaseController).GetControllerName(), new { errorCode = HttpStatusCodes.NotFound });

            courseService.DeleteUserFromCourse(courseId, userId);

            return RedirectToAction(nameof(AdministrationController.UserCourses), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }
    }
}