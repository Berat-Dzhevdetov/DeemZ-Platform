namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using DeemZ.Services.AdminServices;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Services;
    using DeemZ.Services.ResourceService;
    using DeemZ.Services.CourseServices;
    using DeemZ.Models.ViewModels.Resources;

    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IResourceService resourceService;
        private readonly ICourseService courseService;
        private readonly Guard guard;

        public AdministrationController(IAdminService adminService, Guard guard, IResourceService resourceService, ICourseService courseService)
        {
            this.adminService = adminService;
            this.guard = guard;
            this.resourceService = resourceService;
            this.courseService = courseService;
        }

        public IActionResult Index(int page = 1,int quantity = 20)
        {
            var viewModel = adminService.GetIndexPageInfo();

            var allPages = (int)Math.Ceiling(adminService.GetUserCoursesCount() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            viewModel.UserCourses = adminService.GetUserCourses<UserCoursesViewModel>(page,quantity);

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

        public IActionResult Resources(string courseId,int page = 1,int quantity = 20)
        {
            if (guard.AgainstNull(courseId, nameof(courseId))) return BadRequest();

            if (!courseService.GetCourseById(courseId)) return NotFound();

            var resources = (List<IndexResourceViewModel>)resourceService.GetCourseRecourses<IndexResourceViewModel>(courseId);

            var allPages = (int)Math.Ceiling(resources.Count / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            var viewModel = new ResourcesForCourseViewModel();

            viewModel.Recourses = resources.Paging(page,quantity).ToList();

            viewModel = AdjustPages(viewModel, page, allPages);

            viewModel.CourseId = courseId;

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
    }
}