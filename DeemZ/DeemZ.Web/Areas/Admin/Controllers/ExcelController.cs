namespace DeemZ.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.FileService;
    using DeemZ.Services.ExamServices;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize(Roles = AdminRoleName)]
    [Area(AreaNames.AdminArea)]
    public class ExcelController : Controller
    {
        private const string excelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly Guard guard;
        private readonly IExcelService excelService;
        private readonly IExamService examService;

        public ExcelController(Guard guard, IExcelService excelService, IExamService examService)
        {
            this.guard = guard;
            this.excelService = excelService;
            this.examService = examService;
        }

        public IActionResult Download(string examId)
        {
            if (guard.AgainstNull(examId, nameof(examId))) return BadRequest();

            if (!examService.GetExamById(examId)) return NotFound();

            var bytes = excelService.ExportExam(examId);

            return File(bytes, excelMimeType, "report.xlsx");
        }
    }
}
