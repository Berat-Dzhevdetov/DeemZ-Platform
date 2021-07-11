namespace DeemZ.Models.ViewModels.User
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Models.ViewModels.Resources;
    using DeemZ.Models.ViewModels.Surveys;

    public class IndexUserViewModel
    {
        public int Credits { get; set; }
        public IEnumerable<IndexCourseViewModel> Courses { get; set; }
        public IEnumerable<IndexSurveyViewModel> Surveys { get; set; }
        public IEnumerable<IndexResourceViewModel> Resources { get; set; }
        public IEnumerable<IndexSignUpForCourseViewModel> SignUpCourses { get; set; }
    }
}
