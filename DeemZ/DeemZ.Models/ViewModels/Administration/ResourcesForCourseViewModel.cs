namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Resources;

    public class ResourcesForCourseViewModel : PagingBaseModel
    {
        public List<IndexResourceViewModel> Recourses { get; set; }
        public string LectureId { get; set; }
    }
}
