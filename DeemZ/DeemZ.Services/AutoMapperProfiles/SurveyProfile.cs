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

            CreateMap<Survey, DetailsSurveyViewModel>();
        }
    }
}