namespace DeemZ.Services.SurveyServices
{
    using DeemZ.Models.FormModels.Survey;
    using System.Collections.Generic;
    public interface ISurveyService
    {
        IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid);
        T GetSurveyById<T>(string sid);
        bool DoesTheUserHavePermissionToSurvey(string uid, string sid);
        IEnumerable<T> GetSurveysByCourseId<T>(string cid);
        void AddSurveyToCourse(string cid, AddSurveyFormModel survey);
        string EditSurvey(string sid, EditSurveyFormModel survey);
        string DeleteSurvey(string sid);
        IEnumerable<T> GetSurveyQuestions<T>(string sid);
        void AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question);
        T GetQuestionById<T>(string sqid);
        string EditQuestion(string sqid, EditSurveyQuestionFormModel question);
        string DeleteQuestion(string sqid);
    }
}