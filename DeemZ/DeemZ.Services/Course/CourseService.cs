using DeemZ.Data;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<IndexCourseViewModel> GetCoursesByUserId(string id)
        {

            return 1;
        }
    }
}
