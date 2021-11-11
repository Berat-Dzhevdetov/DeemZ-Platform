namespace DeemZ.Services.LectureServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Lecture;
    public interface ILectureService
    {
        T GetLectureById<T>(string lid);
        bool GetLectureById(string lid);
        IEnumerable<T> GetLecturesByCourseId<T>(string cid);
        Task<string> AddLectureToCourse(string courseId,AddLectureFormModel lecture);
        List<T> GetLectureDescriptions<T>(string lid);
        Task EditLectureById(string lectureId, EditLectureFormModel lecture);
        Task DeleteDescription(string did);
        Task DeleteAllDescription(string lid);
        IEnumerable<T> GetLectureResourcesById<T>(string lid);
        Task DeleteLecture(string lid);
    }
}