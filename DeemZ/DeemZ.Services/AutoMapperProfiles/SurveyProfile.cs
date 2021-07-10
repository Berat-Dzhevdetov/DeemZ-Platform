namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Surveys;

    public class SurveyProfile : Profile
    {
        public SurveyProfile()
        {
            CreateMap<Survey, IndexSurveyViewModel>();
            CreateMap<Survey, TakeSurveyViewModel>()
                .ForMember(x => x.Questions, o => o.MapFrom(src => src.Questions));
        }
    }
}
