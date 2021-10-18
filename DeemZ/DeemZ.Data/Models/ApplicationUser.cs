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
        public string ImgPublicId { get; set; }

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
        public ICollection<ApplicationUserExam> Exams { get; set; } = new HashSet<ApplicationUserExam>();
        public ICollection<ApplicationUserSurveyAnswer> SurveyAnswers { get; set; } = new HashSet<ApplicationUserSurveyAnswer>();
        public ICollection<ApplicationUserSurvey> Surveys { get; set; } = new HashSet<ApplicationUserSurvey>();
        public ICollection<AnswerUsers> AnswerUsers { get; set; } = new HashSet<AnswerUsers>();
        public ICollection<Forum> Forums { get; set; } = new HashSet<Forum>();
        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}