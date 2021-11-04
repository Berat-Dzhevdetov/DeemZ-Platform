namespace DeemZ.Models.FormModels.Survey
{
    using System.Collections.Generic;

    public class TakeSurveyQuestionFormModel
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public bool IsOptional { get; set; }
        public List<TakeSurveyAnswerFormModel> Answers { get; set; } = new List<TakeSurveyAnswerFormModel>();
    }
}