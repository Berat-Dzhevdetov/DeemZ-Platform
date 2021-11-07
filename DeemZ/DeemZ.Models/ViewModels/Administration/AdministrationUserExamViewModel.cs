namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Exams;

    public class AdministrationUserExamViewModel : PagingBaseModel
    {
        public IEnumerable<UserExamViewModel> UserExams { get; set; } = new HashSet<UserExamViewModel>();
        public IDictionary<string, string> Exams { get; set; }
        [Display(Name = "Exam")]
        public string ExamId { get; set; }
    }
}
