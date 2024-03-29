﻿namespace DeemZ.Models.DTOs.LiteChat
{
    public class ConnectDTO
    {
        public string ApplicationUserId { get; set; }
        public string CourseId { get; set; }
        public string ApplicationUserImageUrl { get; set; }
        public bool IsAdmin { get; set; }
        public string CourseName { set; get; }
        public string UserName { get; set; }
    }
}