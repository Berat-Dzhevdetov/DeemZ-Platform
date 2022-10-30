namespace DeemZ.Web.Areas.Identity.Pages.Account.Manage
{
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Services.ExamServices;
    using DeemZ.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class UserExamsModel : PageModel
    {
        private readonly IExamService examService;

        public UserExamsModel(IExamService examService)
        {
            this.examService = examService;
        }

        public IActionResult OnGet()
        {
            var userId = User.GetId();

            var exams = examService.GetExamsByUserId<GetUserExamInfoViewModel>(userId);

            foreach (var exam in exams)
                exam.EndDate = exam.EndDate.ToLocalTime();

            return Page();
        }
    }
}
