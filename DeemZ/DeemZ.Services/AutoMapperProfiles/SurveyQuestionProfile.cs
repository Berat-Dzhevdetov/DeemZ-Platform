namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.SurveyQuestions;

    public class SurveyQuestionProfile : Profile
    {
        public SurveyQuestionProfile()
        {
            CreateMap<SurveyQuestion, TakeSurveyQuestionViewModel>()
                .ForMember(x => x.Answers, o => o.MapFrom(src => src.Answers))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Question));
        }
    }
}
