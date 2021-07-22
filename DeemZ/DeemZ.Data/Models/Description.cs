namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Description : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.Description.MaxNameLength)]
        public string Name { get; set; }

        public string LectureId { get; set; }
        public Lecture Lecture { get; set; }
    }
}