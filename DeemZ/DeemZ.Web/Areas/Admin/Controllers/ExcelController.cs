namespace DeemZ.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.FileService;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constants;

    [Authorize(Roles = AdminRoleName)]
    [Area(AreaNames.AdminArea)]
    public class ExcelController : Controller
    {
        private const string excelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IExcelService excelService;

        public ExcelController(IExcelService excelService) => this.excelService = excelService;

        [ClientRequired]
        [IfExists]
        public IActionResult Download(string examId)
        {
            var bytes = excelService.ExportExam(examId);

            return File(bytes, excelMimeType, "report.xlsx");
        }
    }
}
