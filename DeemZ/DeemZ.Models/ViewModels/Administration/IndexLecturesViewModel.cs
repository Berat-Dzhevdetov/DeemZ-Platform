namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Lectures;

    public class IndexLecturesViewModel : PagingBaseModel
    {
        public IEnumerable<LectureBasicInformationViewModel> Lectures { get; set; }
        public string CourseId { get; set; }
    }
}
