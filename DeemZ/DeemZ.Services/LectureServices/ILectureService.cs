namespace DeemZ.Services.LectureServices
{
    public interface ILectureService
    {
        T GetLectureById<T>(string lid);
    }
}