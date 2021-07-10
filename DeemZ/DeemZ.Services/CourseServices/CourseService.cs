namespace DeemZ.Services.CourseServices
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DeemZ.Data;

    public class CourseService : ICourseService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public CourseService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public int GetCreditsByUserId(string id)
            => context
               .Users
               .Include(x => x.Exams)
               .FirstOrDefault(x => x.Id == id)
               .Exams.Sum(x => x.EarnedCredits);


        //Gets user's given id courses
        public IEnumerable<T> GetUserCurrentCourses<T>(string id, bool isPaid = true)
         => context.UserCourses
                .Where(
                    x => x.User.Id == id &&
                    x.IsPaid == isPaid &&
                    x.Course.StartDate <= DateTime.UtcNow &&
                    x.Course.EndDate >= DateTime.UtcNow
                )
                .Include(x => x.Course)
                .Select(x => mapper.Map<T>(x.Course))
                .ToList();

        public T GetCourseById<T>(string id)
            => context.Courses
                .Where(x => x.Id == id)
                .Include(x => x.Lectures)
                .ThenInclude(x => x.Descriptions)
                .Include(c => c.Lectures)
                .ThenInclude(x => x.Resources)
                .ThenInclude(x => x.ResourceType)
                .Select(x => mapper.Map<T>(x))
                .FirstOrDefault();

        public bool IsUserSignUpForThisCourse(string uid, string cid)
            => context.UserCourses
                .Any(x => x.UserId == uid && x.CourseId == cid && x.IsPaid == true);
    }
}