namespace DeemZ.Models.ViewModels.Surveys
{
    public class AllSurveyQuestionsViewModel
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public int AnswersCount { get; set; }
        public bool IsOptional { get; set; }
    }
}
