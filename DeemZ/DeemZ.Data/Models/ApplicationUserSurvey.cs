namespace DeemZ.Data.Models
{
    public class ApplicationUserSurvey
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string SurveyId { get; set; }
        public Survey Survey { get; set; }
    }
}
