namespace DeemZ.Models.FormModels.Exam
{
    using System.Collections.Generic;
    public class TakeExamQuestionFormModel
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public int Points { get; set; }

        public IList<TakeExamQuestionAnswerFormModel> Answers { get; set; } = new List<TakeExamQuestionAnswerFormModel>();
    }
}