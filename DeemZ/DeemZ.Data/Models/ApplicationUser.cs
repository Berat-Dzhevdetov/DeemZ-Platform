namespace DeemZ.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser
    {
        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string FirstName { get; set; }

        [MaxLength(DataConstants.User.MaxLengthForNames)]
        public string LastName { get; set; }

        public bool PrivacyConfirm { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public City City { get; set; }
        public string CityId { get; set; }

        [Required]
        public string ImgUrl { get; set; } = DataConstants.User.DefaultProfilePictureUrl;

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
        public ICollection<ApplicationUserExam> Exams { get; set; } = new HashSet<ApplicationUserExam>();
        public ICollection<ApplicationUserSurvey> Surveys { get; set; } = new HashSet<ApplicationUserSurvey>();
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
        public ICollection<Forum> Forums { get; set; } = new HashSet<Forum>();
    }
}