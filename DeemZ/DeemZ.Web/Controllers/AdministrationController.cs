namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.AdminServices;
    using DeemZ.Models.ViewModels.Administration;

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

            var allPages = adminService.GetUserCoursesCount();

            if (page <= 0 || page > allPages) page = 1;

            viewModel.UserCourses = adminService.GetUserCourses<UserCoursesViewModel>(page,quantity);

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        private AdministrationIndexViewModel AdjustPages(AdministrationIndexViewModel viewModel, int page, int allPages)
        {
            viewModel.CurrentPage = page;
            viewModel.NextPage = page >= allPages ? null : page + 1;
            viewModel.PreviousPage = page <= 1 ? null : page - 1;
            viewModel.MaxPages = allPages;

            return viewModel;
        }
    }
}