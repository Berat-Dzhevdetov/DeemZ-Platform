namespace DeemZ.Models.ViewModels.Administration
{
    using System;

    public class UserCoursesViewModel
    {
        public string UserId { get; set; }
        public string CourseId { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidOn { get; set; }
    }
}
