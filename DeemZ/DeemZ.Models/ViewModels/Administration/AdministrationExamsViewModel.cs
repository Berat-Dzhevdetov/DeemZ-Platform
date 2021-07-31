namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Exams;

    public class AdministrationExamsViewModel
    {
        public string CourseId { get; set; }
        public IEnumerable<BasicExamInfoViewModel> Exams { get; set; }
    }
}