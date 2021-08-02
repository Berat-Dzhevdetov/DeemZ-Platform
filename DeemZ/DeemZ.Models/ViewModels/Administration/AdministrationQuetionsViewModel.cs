namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Exams;

    public class AdministrationQuetionsViewModel : PagingBaseModel
    {
        public IEnumerable<QuetionsViewModel> Questions { get; set; }

        public string ExamName { get; set; }
        public string ExamId { get; set; }
    }
}