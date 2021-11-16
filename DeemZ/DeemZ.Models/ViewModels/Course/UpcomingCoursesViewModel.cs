namespace DeemZ.Models.ViewModels.Course
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;

    public class UpcomingCoursesViewModel : PagingBaseModel
    {
        public IEnumerable<UpcomingCourseViewModel> UpcomingCourses { get; set; }
    }
}