namespace DeemZ.Services.Question
{
    using DeemZ.Models.FormModels.Exam;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionService
    {
        IEnumerable<T> GetQuestionsByExamId<T>(string eid);
        string ValidateQuestionFormModel(AddQuestionFormModel question);
        Task AddQuestionToExam(string eid, AddQuestionFormModel question);
        T GetQuestionById<T>(string qid);
        bool GetQuestionById(string qid);
        Task<string> Delete(string qid);
        Task<string> Edit(string questionId, AddQuestionFormModel question);
    }
}