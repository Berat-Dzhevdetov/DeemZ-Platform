namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class User
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DataConstants.User.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string FirstName { get; set; }
        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public City City { get; set; }
        public string CityId { get; set; }

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
    }
}
