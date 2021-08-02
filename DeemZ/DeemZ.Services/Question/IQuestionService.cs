namespace DeemZ.Services.Question
{
    using DeemZ.Models.FormModels.Exam;
    using System.Collections.Generic;

    public interface IQuestionService
    {
        IEnumerable<T> GetQuestionsByExamId<T>(string eid);
        string ValidateQuestionFormModel(AddQuestionFormModel question);
        void AddQuestionToExam(string eid, AddQuestionFormModel question);
        bool GetQuestionById(string qid);
        string Delete(string qid);
    }
}