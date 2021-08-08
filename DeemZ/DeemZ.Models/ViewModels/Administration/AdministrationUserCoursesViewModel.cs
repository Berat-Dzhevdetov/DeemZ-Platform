namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;

    public class AdministrationUserCoursesViewModel : PagingBaseModel
    {
        public IEnumerable<UserCoursesViewModel> UserCourses { get; set; }
    }
}
