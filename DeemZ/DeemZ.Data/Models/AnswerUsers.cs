namespace DeemZ.Data.Models
{
    public class AnswerUsers
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}