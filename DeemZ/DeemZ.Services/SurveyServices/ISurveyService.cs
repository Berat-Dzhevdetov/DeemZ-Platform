namespace DeemZ.Services.SurveyServices
{
    using DeemZ.Models.FormModels.Survey;
    using System.Collections.Generic;
    public interface ISurveyService
    {
        IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid);
        T GetSurveyById<T>(string sid, bool isPublic = true);
        bool DoesTheUserHavePermissionToSurvey(string uid, string sid);
        IEnumerable<T> GetSurveysByCourseId<T>(string cid);
        void AddSurveyToCourse(string cid, AddSurveyFormModel survey);
    }
}