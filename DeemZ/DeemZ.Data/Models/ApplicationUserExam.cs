namespace DeemZ.Data.Models
{
    using System;
    public class ApplicationUserExam
    {
        public string ExamId { get; set; }
        public Exam Exam { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int EarnedCredits { get; set; }
        public int EarnedPoints { get; set; }
        public DateTime HandOverOn { get; set; } = DateTime.UtcNow;
    }
}