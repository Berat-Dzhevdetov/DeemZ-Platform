namespace DeemZ.Services.ExamServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Exam;

    public interface IExamService
    {
        IEnumerable<T> GetExamsByCourseId<T>(string cid);
        IEnumerable<T> GetExamsByUserId<T>(string uid);
        Task CreateExam(string cid, AddExamFormModel exam);
        bool GetExamById(string eid);
        T GetExamById<T>(string eid);
        Task<string> EditExam(string eid, AddExamFormModel exam);
        bool DoesTheUserHavePermissionToExam(string uid, string eid);
        bool IsProvidedPasswordRight(string eid, string password);
        Task<int> EvaluateExam(TakeExamFormModel exam, string uid);
        Task<int> SaveUserExam(string uid, int points, string eid);
        string GetCourseIdByExamId(string eid);
        IDictionary<string, string> GetUserExamAnswers(string eid, string uid);
        IDictionary<string, string> GetExamsAsKeyValuePair(DateTime prevDate);
        IEnumerable<T> GetUserExams<T>(int page = 1, int quantity = 20);
        IEnumerable<T> GetUserExams<T>(string eid, int page = 1, int quantity = 20);
    }
}