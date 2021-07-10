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

        public bool DoesThisUserHavePermissionToTakeTheSurvey(string uid, string sid)
        {
            var permission = context.Courses
                .Any(x => x.UserCourses.Any(c => c.CourseId == x.Id && c.UserId == uid && c.IsPaid == true));
            var takenOrNot = context.ApplicationUserSurvey
                .Any(x => x.ApplicationUserId == uid && x.SurveyId == sid);
            return permission && !takenOrNot;
        }

        public T GetSurveyById<T>(string sid,bool isPublic = true)
            => context.Surveys
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == sid && x.IsPublic == isPublic)
            .Select(x => mapper.Map<T>(x))
            .FirstOrDefault();

        public IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid)
                => context.Surveys
                .Where(x => x.IsPublic == true
                        && x.Course.UserCourses.Any(x => x.UserId == uid && x.IsPaid == true))
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .Include(x => x.Users)
                .Select(x => mapper.Map<T>(x))
                .ToList();
    }
}
