﻿namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using System.Linq;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Models.ViewModels.Exams;

    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, BasicExamInfoViewModel>()
                .ForMember(x => x.StartDate, o => o.MapFrom(src => src.StartDate.ToLocalTime()))
                .ForMember(x => x.EndDate, o => o.MapFrom(src => src.EndDate.ToLocalTime()));

            CreateMap<Exam, GetUserExamInfoViewModel>()
                .ForMember(x => x.StartDate, o => o.MapFrom(src => src.StartDate.ToLocalTime()))
                .ForMember(x => x.EndDate, o => o.MapFrom(src => src.EndDate.ToLocalTime()));

            CreateMap<AddExamFormModel, Exam>()
                .ReverseMap();

            CreateMap<Question, QuetionsViewModel>();

            CreateMap<AddQuestionFormModel, Question>()
                .ReverseMap();

            CreateMap<AddAnswerFormModel, Answer>()
                .ReverseMap();

            CreateMap<Exam, TakeExamFormModel>()
                .ReverseMap();

            CreateMap<Question, TakeExamQuestionFormModel>()
                .ReverseMap();

            CreateMap<Answer, TakeExamQuestionAnswerFormModel>()
                .ReverseMap();

            CreateMap<Description, Description>();

            CreateMap<Exam, ViewExamViewModel>();

            CreateMap<Question, ViewExamQuestionViewModel>();

            CreateMap<Answer, ViewExamQuestionAnswersViewModel>();
            CreateMap<ApplicationUserExam, UserExamViewModel>()
                .ForMember(x => x.MaxPoints, o => o.MapFrom(src => src.Exam.Questions.Sum(x => x.Points)));
            CreateMap<ApplicationUserExam, GetUserExamInfoViewModel>();

        }
    }
}