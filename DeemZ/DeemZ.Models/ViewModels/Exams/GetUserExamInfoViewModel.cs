namespace DeemZ.Models.ViewModels.Exams
{
    using System;
    using Data.Models;
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Exam;

    public class GetUserExamInfoViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsPublic { get; set; } = false;

        public Course Course { get; set; }

        public IList<TakeExamQuestionFormModel> Questions { get; set; }

        public ICollection<ApplicationUserExam> Users { get; set; }
    }
}