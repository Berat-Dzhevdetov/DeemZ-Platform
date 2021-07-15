namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using DeemZ.Data.Models;
    using DeemZ.Services.LectureServices;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Services;
    using DeemZ.Services.ReportService;

    public class ReportController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILectureService lectureService;
        private readonly IReportService reportService;
        private readonly Guard guard;

        public ReportController(UserManager<ApplicationUser> userManager, ILectureService lectureService, Guard guard, IReportService reportService)
        {
            this.userManager = userManager;
            this.lectureService = lectureService;
            this.guard = guard;
            this.reportService = reportService;
        }

        public IActionResult Add(string lectureId)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return NotFound();

            var model = new AddReportFormModel()
            {
                Id = lectureId,
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddReportFormModel formModel)
        {
            if (!ModelState.IsValid) return View(formModel);

            var lecture = lectureService.GetLectureById<AddReportFormModel>(formModel.Id);

            if (lecture == null) return NotFound();

            var user = await userManager.GetUserAsync(HttpContext.User);

            reportService.AddReport(formModel, user.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}
