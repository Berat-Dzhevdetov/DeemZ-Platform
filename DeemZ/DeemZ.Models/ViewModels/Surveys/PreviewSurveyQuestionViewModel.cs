namespace DeemZ.Models.ViewModels.Surveys
{
    using System.Collections.Generic;
    public class PreviewSurveyQuestionViewModel
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public IEnumerable<AllSurveyQuestionAnswersViewModel> Answers { get; set; } = new HashSet<AllSurveyQuestionAnswersViewModel>();
    }
}