namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.SurveyQuestions;
    using System.Linq;

    public class SurveyQuestionProfile : Profile
    {
        public SurveyQuestionProfile()
        {
            CreateMap<SurveyQuestion, TakeSurveyQuestionViewModel>()
                .ForMember(x => x.Answers, o => o.MapFrom(src => src.Answers.OrderBy(x => x.Text)))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Question));
        }
    }
}
