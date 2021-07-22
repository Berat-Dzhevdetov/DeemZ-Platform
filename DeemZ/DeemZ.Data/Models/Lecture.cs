namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Lecture : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.Lecture.MaxNameLength)]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public string CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Description> Descriptions { get; set; } = new HashSet<Description>();

        public ICollection<Resource> Resources { get; set; } = new HashSet<Resource>();
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}
