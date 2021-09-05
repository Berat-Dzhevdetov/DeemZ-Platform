namespace DeemZ.Models.ViewModels.Exams
{
    using System;
    using System.Collections.Generic;

    public class ViewExamViewModel
    {
        public string Name { get; set; }

        public DateTime EndDate { get; set; }

        public IList<ViewExamQuestionViewModel> Questions { get; set; } = new List<ViewExamQuestionViewModel>();
        public IDictionary<string, string> UserAnswers { get; set; } = new Dictionary<string, string>();
    }
}