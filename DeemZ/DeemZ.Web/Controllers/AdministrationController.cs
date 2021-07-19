namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.AdminServices;
    using DeemZ.Models.ViewModels.Administration;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IAdminService adminService;

        public AdministrationController(IAdminService adminService)
        {
            this.adminService = adminService;
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