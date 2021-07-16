namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.AdminServices;

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
            return View(viewModel);
        }
    }
}