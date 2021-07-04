namespace DeemZ.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(DataConstants.User.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string FirstName { get; set; }
        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string LastName { get; set; }

        public City City { get; set; }
        public string CityId { get; set; }

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();

    }
}
