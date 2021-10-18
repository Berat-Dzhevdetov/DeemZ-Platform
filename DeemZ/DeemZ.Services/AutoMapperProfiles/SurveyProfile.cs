namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Models.ViewModels.Surveys;

    public class SurveyProfile : Profile
    {
        public SurveyProfile()
        {
            CreateMap<Survey, IndexSurveyViewModel>();

            CreateMap<Survey, DetailsSurveyViewModel>();

            CreateMap<Survey, EditSurveyFormModel>();

            CreateMap<SurveyQuestion, AllSurveyQuestionsViewModel>()
                .ForMember(x => x.AnswersCount, o => o.MapFrom(src => src.Answers.Count));
        }
    }
}