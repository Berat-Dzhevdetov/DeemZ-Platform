namespace DeemZ.Models.FormModels.Survey
{
    using System.ComponentModel.DataAnnotations;
    public class TakeSurveyAnswerFormModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool IsChosen { get; set; }
    }
}