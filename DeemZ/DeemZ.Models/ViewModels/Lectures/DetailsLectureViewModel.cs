namespace DeemZ.Models.ViewModels.Lectures
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Description;
    using DeemZ.Models.ViewModels.Resources;

    public class DetailsLectureViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<DetailsDescriptionViewModel> Descriptions { get; set; }
        public List<DetailsResourseViewModel> Resourses { get; set; }
    }
}
