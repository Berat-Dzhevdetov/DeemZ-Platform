namespace DeemZ.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.FileService;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Services.CourseServices;
    using DeemZ.Data.Models;
    using DeemZ.Services.ExamServices;

    [Authorize(Roles = Role.AdminRoleName)]
    [Area(AreaName.AdminArea)]
    public class ExcelController : Controller
    {
        private const string excelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IExcelService excelService;
        private readonly ICourseService courseService;
        private readonly IExamService examService;

        public ExcelController(IExcelService excelService, ICourseService courseService, IExamService examService)
        {
            this.excelService = excelService;
            this.courseService = courseService;
            this.examService = examService;
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Download(string examId)
        {
            var bytes = excelService.ExportExam(examId);

            var courseId = examService.GetExamById<Exam>(examId).CourseId;

            var courseName = courseService.GetCourseById<Course>(courseId).Name;

            return File(bytes, excelMimeType, $"{courseName}.xlsx");
        }
    }
}
