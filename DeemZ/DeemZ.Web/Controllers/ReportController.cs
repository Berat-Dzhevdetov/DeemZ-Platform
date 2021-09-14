namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services.LectureServices;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Services.ReportService;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.Reports;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constants;

    public class ReportController : Controller
    {
        private readonly ILectureService lectureService;
        private readonly IReportService reportService;

        public ReportController(ILectureService lectureService, IReportService reportService)
        {
            this.lectureService = lectureService;
            this.reportService = reportService;
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public IActionResult Add(string lectureId)
        {
            var model = new AddReportFormModel()
            {
                LectureId = lectureId,
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ClientRequired]
        public IActionResult Add(AddReportFormModel formModel)
        {
            if (!ModelState.IsValid) return View(formModel);

            var lecture = lectureService.GetLectureById<AddReportFormModel>(formModel.LectureId);

            if (lecture == null) return NotFound();

            var userId = User.GetId();

            reportService.AddReport(formModel, userId);

            TempData[GlobalMessageKey] = "Your report was successfuly sent!";

            return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
        }

        [Authorize(Roles = AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Preview(string reportId)
        {
            var report = reportService.GetReportById<PreviewReportViewModel>(reportId);

            return View(report);
        }

        [Authorize(Roles = AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string reportId)
        {
            var report = reportService.GetReportById(reportId);

            reportService.Delete(reportId);

            return RedirectToAction(nameof(AdministrationController.Reports), typeof(HomeController).GetControllerName());
        }
    }
}