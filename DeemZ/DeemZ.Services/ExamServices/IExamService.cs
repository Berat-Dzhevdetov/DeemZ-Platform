namespace DeemZ.Services.ExamServices
{
    using DeemZ.Models.FormModels.Exam;
    using System.Collections.Generic;
    public interface IExamService
    {
        IEnumerable<T> GetExamsByCourseId<T>(string cid);
        void CreateExam(string cid, AddExamFormModel exam);
        bool GetExamById(string eid);
        T GetExamById<T>(string eid);
        string EditExam(string examId, AddExamFormModel exam);
    }
}