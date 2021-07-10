namespace DeemZ.Models.ViewModels.Surveys
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.SurveyQuestions;

    public class TakeSurveyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TakeSurveyQuestionViewModel> Questions { get; set; }
    }
}
