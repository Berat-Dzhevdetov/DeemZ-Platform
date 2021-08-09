namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.LectureServices;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Services.UserServices;

    using static Global.WebConstants.Constants;

    public class CourseController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;
        private readonly ILectureService lectureService;
        private readonly IUserService userService;

        public CourseController(Guard guard, ICourseService courseService, ILectureService lectureService, IUserService userService)
        {
            this.guard = guard;
            this.courseService = courseService;
            this.lectureService = lectureService;
            this.userService = userService;
        }

        public IActionResult ViewCourse(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            var course = courseService.GetCourseById<DetailsCourseViewModel>(courseId);

            course.StartDate = course.StartDate.ToLocalTime();

            var userId = User.GetId();

            if (userId == null) course.IsUserSignUpForThisCourse = false;
            else course.IsUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            return View(course);
        }

        public IActionResult SignUp(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SignUp(string courseId, SignUpCourseFormModel signUp)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            var userId = User.GetId();

            var isUserSignUpForThisCourse = courseService.IsUserSignUpForThisCourse(userId, courseId);

            if (isUserSignUpForThisCourse) return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });

            if (!ModelState.IsValid) return View(signUp);

            var course = courseService.GetCourseById<SignUpCourseFormModel>(courseId);

            if (course == null) return BadRequest();

            courseService.SignUserToCourse(userId, course.Id);

            return RedirectToAction(nameof(ViewCourse), new { courseId = courseId });
        }

        public IActionResult Add() => View();


        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add(AddCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            var courseId = courseService.CreateCourse(course);

            if (course.BasicLectures) courseService.CreateBasicsLectures(courseId, course);

            if (course.Redirect) return RedirectToAction(nameof(CourseController.ViewCourse), typeof(CourseController).GetControllerName(), new { courseId });

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string courseId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            var course = courseService.GetCourseById<EditCourseFormModel>(courseId);

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Edit(string courseId, EditCourseFormModel course)
        {
            if (!ModelState.IsValid) return View(course);

            courseService.Edit(course, courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Delete(string courseId)
        {
            if (guard.AgainstNull(courseId)) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            courseService.DeleteCourse(courseId);

            return RedirectToAction(nameof(AdministrationController.Courses), typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult AddUserToCourse()
        {
            var model = new AddUserToCourseFormModel();

            var prevDate = DateTime.Now.AddDays(-14);

            model.Courses = courseService.GetCourseByIdAsKeyValuePair(prevDate);

            return View(model);
        }

        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
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

            return RedirectToAction(nameof(AdministrationController.UserCourses),typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea });
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult DeleteUserFromCourse(string courseId, string userId)
        {
            if (guard.AgainstNull(courseId, nameof(courseId)) ||
                guard.AgainstNull(userId, nameof(userId))) return BadRequest();

            if (!courseService.GetCourseById(courseId) ||
                !userService.GetUserById(userId)) return NotFound();

            courseService.DeleteUserFromCourse(courseId, userId);

            return RedirectToAction(nameof(AdministrationController.UserCourses), typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea });
        }
    }
}