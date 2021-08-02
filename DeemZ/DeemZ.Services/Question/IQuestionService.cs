namespace DeemZ.Services.Question
{
    using DeemZ.Models.FormModels.Exam;
    using System.Collections.Generic;

    public interface IQuestionService
    {
        IEnumerable<T> GetQuestionsByExamId<T>(string examId);
        string ValidateQuestionFormModel(AddQuestionFormModel question);
        void AddQuestionToExam(string examId, AddQuestionFormModel question);
    }
}