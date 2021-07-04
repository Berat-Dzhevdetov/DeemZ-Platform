namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InformativeMessage
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime ShowFrom { get; set; } = DateTime.UtcNow;
        public DateTime ShowTo { get; set; } = DateTime.UtcNow.AddDays(7);

    }
}
