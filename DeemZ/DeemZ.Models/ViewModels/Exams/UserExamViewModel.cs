namespace DeemZ.Models.ViewModels.Exams
{
    using System;

    public class UserExamViewModel
    {
        public string ApplicationUserUserName { get; set; }
        public int EarnedCredits { get; set; }
        public int EarnedPoints { get; set; }
        public int MaxPoints { get; set; }
        public DateTime HandOverOn { get; set; }
    }
}
