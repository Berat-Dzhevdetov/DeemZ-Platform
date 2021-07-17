namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;

    public class AdministrationCoursesViewModel : PagingBaseModel
    {
        public List<CoursesViewModel> Courses { get; set; }
    }
}