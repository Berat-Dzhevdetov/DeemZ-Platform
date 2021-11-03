namespace DeemZ.Models.FormModels.Survey
{
    using System;
    using System.Collections.Generic;

    public class TakeSurveyFormModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<TakeSurveyQuestionFormModel> Questions { get; set; } = new List<TakeSurveyQuestionFormModel>();
    }
}