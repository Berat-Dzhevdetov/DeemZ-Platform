using DeemZ.Data;
using DeemZ.Models.ViewModels.Course;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeemZ.Services.Course
{
    public class CourseService : ICourseService
    {
        private DeemZDbContext context;
        public CourseService(DeemZDbContext context)
        {
            this.context = context;
        }

        public int GetCreditsByUserId(string id)
            => context
               .Users
               .Include(x => x.Exams)
               .FirstOrDefault(x => x.Id == id)
               .Exams.Sum(x => x.Credits);

        public IEnumerable<IndexCourseViewModel> GetCurrentCoursesByUserId(string id)
        {
            context.Courses
                .Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow)
                .Select(x => new IndexCourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            return new List<IndexCourseViewModel>();
        }
    }
}
