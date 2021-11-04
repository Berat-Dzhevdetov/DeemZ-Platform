namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;

    public class AdministrationCoursesViewModel : PagingBaseModel
    {
        public List<CoursesViewModel> Courses { get; set; }
    }
}