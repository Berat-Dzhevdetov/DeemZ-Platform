namespace DeemZ.Services.SurveyServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Data.Models;

    public class SurveyService : ISurveyService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public SurveyService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool DoesTheUserHavePermissionToSurvey(string uid, string sid)
        {
            var permission = context.Courses
                .Any(x => x.UserCourses.Any(c => c.CourseId == x.Id && c.UserId == uid && c.IsPaid == true));
            var takenOrNot = context.ApplicationUserSurveys
                .Any(x => x.ApplicationUserId == uid && x.SurveyId == sid);
            return permission && !takenOrNot;
        }

        public T GetSurveyById<T>(string sid,bool isPublic = true)
            => context.Surveys
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == sid && x.IsPublic == isPublic)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        public IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid)
                => context.Surveys
                .Where(x => x.IsPublic == true
                        && x.Course.UserCourses.Any(x => x.UserId == uid && x.IsPaid == true))
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetSurveysByCourseId<T>(string cid)
            => context.Surveys
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public void AddSurveyToCourse(string cid, AddSurveyFormModel survey)
        {
            var newlySurvey = new Survey
            {
                CourseId = cid,
                StartDate = survey.StartDate.ToUniversalTime(),
                EndDate = survey.EndDate.ToUniversalTime(),
                Name = survey.Name,
                IsPublic = survey.IsPublic
            };

            context.Surveys.Add(newlySurvey);

            context.SaveChanges();
        }
    }
}
