namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;

    public class AdministrationUserCoursesViewModel : PagingBaseModel
    {
        public IEnumerable<UserCoursesViewModel> UserCourses { get; set; }
    }
}
