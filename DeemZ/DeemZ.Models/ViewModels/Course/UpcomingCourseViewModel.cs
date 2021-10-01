using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.ViewModels.Course
{
    public class UpcomingCourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Credits { get; set; }

        public DateTime SignUpStartDate { get; set; } = DateTime.UtcNow;
        public DateTime SignUpEndDate { get; set; } = DateTime.UtcNow.AddDays(14);
    }
}
