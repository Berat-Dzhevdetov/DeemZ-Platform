namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Area : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public string CountryId { get; set; }
    }
}