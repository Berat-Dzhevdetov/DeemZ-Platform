namespace DeemZ.Models.ViewModels.User
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.Homework;
    using DeemZ.Models.ViewModels.Resources;

    public class IndexUserViewModel
    {
        public int Credits { get; set; }
        public IEnumerable<IndexCourseViewModel> Courses { get; set; }
        //public List<IndexHomeworkViewModel> Homework { get; set; }
        //public List<IndexResourceViewModel> Resources { get; set; }
    }
}
