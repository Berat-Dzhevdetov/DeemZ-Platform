namespace DeemZ.Data.Models
{
    public class ApplicationUserSurveyAnswer
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string SurveyAnswerId { get; set; }
        public SurveyAnswer SurveyAnswer { get; set; }
    }
}