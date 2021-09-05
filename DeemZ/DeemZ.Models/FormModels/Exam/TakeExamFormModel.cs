using DeemZ.Infrastructure.Attributes;

namespace DeemZ.Models.FormModels.Exam
{
    using System;
    using System.Collections.Generic;

    public class TakeExamFormModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Password { get; set; }

        public bool ShuffleQuestions { get; set; } = true;

        public bool ShuffleAnswers { get; set; } = true;

        public bool IsPublic { get; set; } = false;

        public IList<TakeExamQuestionFormModel> Questions { get; set; }
        public IList<TakeExamQuestionFormModel> Questions { get; set; } = new List<TakeExamQuestionFormModel>();

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}