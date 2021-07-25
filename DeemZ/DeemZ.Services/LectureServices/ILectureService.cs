namespace DeemZ.Services.LectureServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Lecture;
    public interface ILectureService
    {
        T GetLectureById<T>(string lid);
        bool GetLectureById(string lid);
        IEnumerable<T> GetLecturesByCourseId<T>(string cid);
        void AddLectureToCourse(string courseId,AddLectureFormModel lecture);
        IEnumerable<T> GetLectureDescriptions<T>(string lid);
        void EditLectureById(string lectureId, EditLectureFormModel lecture);
        void DeleteDescription(string did);
    }
}