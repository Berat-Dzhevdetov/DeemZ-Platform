namespace DeemZ.Services.ExamServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Exam;

    public interface IExamService
    {
        IEnumerable<T> GetExamsByCourseId<T>(string cid);
        void CreateExam(string cid, AddExamFormModel exam);
        bool GetExamById(string eid);
        T GetExamById<T>(string eid);
        string EditExam(string eid, AddExamFormModel exam);
        bool DoesTheUserHavePermissionToExam(string uid, string eid);
        bool IsProvidedPasswordRight(string eid, string password);
        int EvaluateExam(TakeExamFormModel exam);
        int SaveUserExam(string uid, int points, string eid);
    }
}