﻿namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Models.ViewModels.Exams;

    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, BasicExamInfoViewModel>();

            CreateMap<AddExamFormModel, Exam>()
                .ForMember(x => x.StartDate, o => o.MapFrom(src => src.StartDate.ToUniversalTime()))
                .ForMember(x => x.EndDate, o => o.MapFrom(src => src.EndDate.ToUniversalTime()));
        }
    }
}