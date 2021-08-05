namespace DeemZ.Models.ViewModels.Exams
{
    using System;

    public class BasicExamInfoViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}