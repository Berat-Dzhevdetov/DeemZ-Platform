﻿namespace DeemZ.Models.ViewModels.Lectures
{
    using System;
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Description;
    using DeemZ.Models.ViewModels.Resources;

    public class DetailsLectureViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<DetailsDescriptionViewModel> Descriptions { get; set; }
        public List<BasicDetailsResourseViewModel> Resourses { get; set; }
    }
}
