namespace DeemZ.Models.ViewModels.Exams
{
    using System.Collections.Generic;

    public class ViewExamQuestionViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public int Points { get; set; }

        public IList<ViewExamQuestionAnswersViewModel> Answers { get; set; } = new List<ViewExamQuestionAnswersViewModel>();
    }
}