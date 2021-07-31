namespace DeemZ.Models.ViewModels.Course
{
    using System;
    public class IndexSignUpForCourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public DateTime SignUpStartDate { get; set; }
        public DateTime SignUpEndDate { get; set; }
    }
}