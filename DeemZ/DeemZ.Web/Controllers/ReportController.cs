namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Services.LectureServices;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Services;
    using DeemZ.Services.ReportService;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.Reports;
    using DeemZ.Web.Areas.Administration.Controllers;

    using static DeemZ.Global.WebConstants.Constants;

    public class ReportController : Controller
    {
        private readonly ILectureService lectureService;
        private readonly IReportService reportService;
        private readonly Guard guard;

        public ReportController(ILectureService lectureService, Guard guard, IReportService reportService)
        {
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

        [HttpPost]
        public IActionResult Add(AddReportFormModel formModel)
        {
            if (!ModelState.IsValid) return View(formModel);

            var lecture = lectureService.GetLectureById<AddReportFormModel>(formModel.Id);

            if (lecture == null) return NotFound();

            var userId = User.GetId();

            reportService.AddReport(formModel, userId);

            TempData[GlobalMessageKey] = "Your report was successfuly sent!";

            return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Preview(string reportId)
        {
            if (guard.AgainstNull(reportId, nameof(reportId))) return BadRequest();

            var report = reportService.GetReportById<PreviewReportViewModel>(reportId);

            if (report == null) return NotFound();

            return View(report);
        }

        [Authorize(Roles = AdminRoleName)]
        [HttpPost]
        public IActionResult Delete(string reportId)
        {
            if (guard.AgainstNull(reportId, nameof(reportId))) return BadRequest();

            var report = reportService.GetReportById(reportId);

            if (!report) return NotFound();

            reportService.Delete(reportId);

            return RedirectToAction(nameof(AdministrationController.Reports), typeof(HomeController).GetControllerName());
        }
    }
}