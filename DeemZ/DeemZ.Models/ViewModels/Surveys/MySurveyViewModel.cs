namespace DeemZ.Models.ViewModels.Surveys
{
    using System.Collections.Generic;
    public class MySurveyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PreviewSurveyQuestionViewModel> Questions { get; set; } = new HashSet<PreviewSurveyQuestionViewModel>();
        public IDictionary<string, string> UserAnswers { get; set; } = new Dictionary<string, string>();
    }
}
