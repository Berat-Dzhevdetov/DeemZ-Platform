namespace DeemZ.Models.ViewModels.User
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Course;

    public class IndexUserViewModel
    {
        public int Credits { get; set; }
        public IEnumerable<IndexCourseViewModel> Courses { get; set; }

        public IEnumerable<IndexSurveyViewModel> Survey { get; set; }
    }
}
