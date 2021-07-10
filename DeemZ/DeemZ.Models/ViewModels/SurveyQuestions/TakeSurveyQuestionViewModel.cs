namespace DeemZ.Models.ViewModels.SurveyQuestions
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.SurveyAnswers;
    public class TakeSurveyQuestionViewModel
    {
        public string Name { get; set; }
        public List<TakeSurveyAnswerViewModel> Answers { get; set; }
    }
}
