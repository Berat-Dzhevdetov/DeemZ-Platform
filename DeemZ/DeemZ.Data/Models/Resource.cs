﻿namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Resource : BaseModel
    {
        [Required]
        public string Path { get; set; }

        [Required]
        [MaxLength(DataConstants.Resource.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public string PublicId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string LectureId { get; set; }
        public Lecture Lecture { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }
        [Required]
        public string ResourceTypeId { get; set; }
    }
}