namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.SurveyAnswers;

    public class SurveyAnswerProfile : Profile
    {
        public SurveyAnswerProfile()
        {
            CreateMap<SurveyAnswer, TakeSurveyAnswerViewModel>();
        }
    }
}
