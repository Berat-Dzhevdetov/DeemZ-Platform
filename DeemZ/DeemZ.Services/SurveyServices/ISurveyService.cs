namespace DeemZ.Services.SurveyServices
{
    using System.Collections.Generic;
    public interface ISurveyService
    {
        IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid);
    }
}