namespace DeemZ.Data.Models
{
    using System.Collections.Generic;
    public class ApplicationUserSurvey
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string SurveyId { get; set; }
        public Survey Survey { get; set; }

        public ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
    }
}