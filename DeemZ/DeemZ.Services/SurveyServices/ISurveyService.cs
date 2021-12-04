namespace DeemZ.Services.SurveyServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Survey;

    public interface ISurveyService
    {
        Task<IEnumerable<T>> GetUserCurrentCourseSurveys<T>(string uid, bool isAdmin);
        Task<T> GetSurveyById<T>(string sid);
        bool DoesTheUserHavePermissionToSurvey(string uid, string sid);
        Task<IEnumerable<T>> GetSurveysByCourseId<T>(string cid);
        Task AddSurveyToCourse(string cid, AddSurveyFormModel survey);
        Task<string> EditSurvey(string sid, EditSurveyFormModel survey);
        Task<string> DeleteSurvey(string sid);
        Task<IEnumerable<T>> GetSurveyQuestions<T>(string sid);
        Task<string> AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question);
        Task<T> GetQuestionById<T>(string sqid);
        Task<string> EditQuestion(string sqid, EditSurveyQuestionFormModel question);
        Task<string> DeleteQuestion(string sqid);
        Task<IEnumerable<T>> GetAllAnswers<T>(string sqid);
        Task AddAnswerToQuestion(string sqid, AddSurveyAnswerFormModel answer);
        Task<T> GetAnswerById<T>(string said);
        Task<string> EditAnswer(string said, EditSurveyAnswerFormModel answer);
        Task<string> DeleteAnswer(string said);
        bool CanUserAccessSurveyById(string sid, string uid);
        void PrepareSurvey(string sid, out TakeSurveyFormModel survey);
        (Dictionary<string, string>, List<string>) ValidateSurvey(TakeSurveyFormModel survey);
        Task SaveSurvey(string sid, string uid,List<string> correctAnswerIds);
        Task AddRatingScaleToQuestion(string sqid);
        IEnumerable<T> GetUserAllSurveys<T>(string uid, int page = 1);
        Task<IDictionary<string, string>> GetUserAnswers(string uid, string sid);
        Task<int> GetUserAllSurveysCount(string userId);
    }
}