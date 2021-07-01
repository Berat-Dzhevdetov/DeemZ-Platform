namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class City
    {
        [Key]
        [Required]
        [MaxLength(DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public Area Area { get; set; }

        [Required]
        public string AreaId { get; set; }
    }
}
