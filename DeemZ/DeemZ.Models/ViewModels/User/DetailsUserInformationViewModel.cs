namespace DeemZ.Models.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Models.ViewModels.Certificates;
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Models.ViewModels.Surveys;

    public class DetailsUserInformationViewModel
    {
        public string Telephone { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ImgUrl { get; set; }
        public IEnumerable<UserCoursesViewModel> UserCourses { get; set; }
        public IEnumerable<GetUserExamInfoViewModel> Exams { get; set; }
        public IEnumerable<IndexSurveyViewModel> Surveys { get; set; }
        public IEnumerable<CertificateDetailsViewModel> Certificates { set; get; }
    }
}
