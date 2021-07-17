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

        public IActionResult Index()
        {
            var viewModel = adminService.GetIndexPageInfo();
            viewModel.UserCourses = adminService.GetUserCourses<UserCoursesViewModel>();
            
            return View(viewModel);
        }
    }
}