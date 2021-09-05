namespace DeemZ.Models.ViewModels.Exams
{
    public class ViewExamQuestionAnswersViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool IsChosen { get; set; }
        public bool IsCorrect { get; set; }
    }
}