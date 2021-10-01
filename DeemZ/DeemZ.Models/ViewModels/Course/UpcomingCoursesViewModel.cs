using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.ViewModels.Course
{
    public class UpcomingCoursesViewModel
    {
        public IEnumerable<UpcomingCourseViewModel> UpcomingCourses { get; set; }
    }
}
