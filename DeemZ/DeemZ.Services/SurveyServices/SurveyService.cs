namespace DeemZ.Services.SurveyServices
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using DeemZ.Data;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class SurveyService : ISurveyService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public SurveyService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid)
                => context.Surveys
                .Where(x => x.IsPublic == true
                        && x.Course.UserCourses.Any(x => x.UserId == uid && x.IsPaid == true))
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .Select(x => mapper.Map<T>(x))
                .ToList();
    }
}
