namespace DeemZ.Models.ViewModels.Surveys
{
    using System;
    public class DetailsSurveyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
