﻿namespace DeemZ.Models.ViewModels.Course
{
    using System;
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Exams;
    using DeemZ.Models.ViewModels.Lectures;

    public class DetailsCourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int Credits { get; set; }
        public string Description { get; set; }

        public DateTime SignUpStartDate { get; set; }
        public DateTime SignUpEndDate { get; set; }

        public List<DetailsLectureViewModel> Lectures { get; set; }

        public List<BasicExamInfoViewModel> Exams { get; set; }

        public bool IsUserSignUpForThisCourse { get; set; }
        public string SuitableForDescription { get; set; }
        public string StartDateDescription { get; set; }
        public string LectureDescription { get; set; }
        public string ExamDescription { get; set; }

    }
}