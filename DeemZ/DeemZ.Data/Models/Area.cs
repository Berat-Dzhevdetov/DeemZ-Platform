namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Area
    {
        [Key]
        [Required]
        [MaxLength(DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public Country Country { get; set; }

        [Required]
        public string CountryId { get; set; }
    }
}
