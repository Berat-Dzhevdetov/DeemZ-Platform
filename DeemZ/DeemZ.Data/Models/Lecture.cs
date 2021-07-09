namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Lecture
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public ICollection<Description> Descriptions { get; set; } = new HashSet<Description>();

        public ICollection<Resource> Resources { get; set; } = new HashSet<Resource>();
    }
}
