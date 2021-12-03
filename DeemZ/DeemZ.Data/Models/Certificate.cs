namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Certificate : BaseModel
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string PublicId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public int ExternalNumber { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; }
    }
}
