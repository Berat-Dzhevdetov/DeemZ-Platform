namespace DeemZ.Services.CourseServices
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DeemZ.Data;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Data.Models;

    public class CourseService : ICourseService
    {
        private DeemZDbContext context;
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
               .Exams.Sum(x => x.Credits);

        
        //Gets user's given id courses
        public IEnumerable<T> GetCurrentCoursesByUserId<T>(string id, bool isPaid = true)
         => context.UserCourses
                .Where(
                    x => x.User.Id == id &&
                    x.IsPaid == isPaid 
                    //x.Course.StartDate <= DateTime.UtcNow &&
                    //x.Course.EndDate >= DateTime.UtcNow 
                )
                .Include(x => x.Course)
                .Select(x => mapper.Map<T>(x.Course))
                .ToList();
    }
}