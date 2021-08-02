namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using System.Linq;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.SurveyQuestions;
    using DeemZ.Models.ViewModels.Surveys;
    using DeemZ.Models.ViewModels.SurveyAnswers;

    public class SurveyProfile : Profile
    {
        public SurveyProfile()
        {
            CreateMap<Survey, IndexSurveyViewModel>();

            CreateMap<Survey, TakeSurveyViewModel>()
                .ForMember(x => x.Questions, o => o.MapFrom(src => src.Questions));

            CreateMap<SurveyQuestion, TakeSurveyQuestionViewModel>()
                .ForMember(x => x.Answers, o => o.MapFrom(src => src.Answers.OrderBy(x => x.Text)))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Question));

            CreateMap<SurveyAnswer, TakeSurveyAnswerViewModel>();
        }
    }
}