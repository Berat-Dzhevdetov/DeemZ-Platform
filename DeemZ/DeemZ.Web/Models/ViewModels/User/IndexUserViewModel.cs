namespace DeemZ.Web.Models.ViewModels.User
{
    using System.Collections.Generic;
    using DeemZ.Web.Models.ViewModels.Course;
    using DeemZ.Web.Models.ViewModels.Homework;
    using DeemZ.Web.Models.ViewModels.Resources;

    public class IndexUserViewModel
    {
        public int Credits { get; set; }
        public List<IndexCourseViewModel> Courses { get; set; }
        public List<IndexHomeworkViewModel> Homework { get; set; }
        public List<IndexResourceViewModel> Resources { get; set; }
    }
}
