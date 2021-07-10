namespace DeemZ.Models.ViewModels.Course
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Lectures;

    public class DetailsCourseViewModel
    {
        public string Name { get; set; }
        public string StartDate { get; set; }

        public int Credits { get; set; }

        public List<DetailsLectureViewModel> Lectures { get; set; }
    }
}