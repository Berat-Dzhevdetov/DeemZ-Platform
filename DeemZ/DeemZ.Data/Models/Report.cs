namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Report : BaseModel
    {
        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string IssueDescription { get; set; }
    }
}