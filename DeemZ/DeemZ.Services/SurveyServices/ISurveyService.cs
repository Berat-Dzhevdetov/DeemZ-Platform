namespace DeemZ.Services.SurveyServices
{
    using System.Collections.Generic;
    public interface ISurveyService
    {
        IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid);
        T GetSurveyById<T>(string sid, bool isPublic = true);
        bool DoesThisUserHavePermissionToTakeTheSurvey(string uid, string sid);
    }
}