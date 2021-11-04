namespace DeemZ.Services.SurveyServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Survey;

    public interface ISurveyService
    {
        IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid, bool isAdmin);
        T GetSurveyById<T>(string sid);
        bool DoesTheUserHavePermissionToSurvey(string uid, string sid);
        IEnumerable<T> GetSurveysByCourseId<T>(string cid);
        void AddSurveyToCourse(string cid, AddSurveyFormModel survey);
        string EditSurvey(string sid, EditSurveyFormModel survey);
        string DeleteSurvey(string sid);
        IEnumerable<T> GetSurveyQuestions<T>(string sid);
        string AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question);
        T GetQuestionById<T>(string sqid);
        string EditQuestion(string sqid, EditSurveyQuestionFormModel question);
        string DeleteQuestion(string sqid);
        IEnumerable<T> GetAllAnswers<T>(string sqid);
        void AddAnswerToQuestion(string sqid, AddSurveyAnswerFormModel answer);
        T GetAnswerById<T>(string said);
        string EditAnswer(string said, EditSurveyAnswerFormModel answer);
        string DeleteAnswer(string said);
        bool CanUserAccessSurveyById(string sid, string uid);
        void PrepareSurvey(string sid, out TakeSurveyFormModel survey);
        (Dictionary<string, string>, List<string>) ValidateSurvey(TakeSurveyFormModel survey);
        void SaveSurvey(string sid, string uid,List<string> correctAnswerIds);
        void AddRatingScaleToQuestion(string sqid);
    }
}