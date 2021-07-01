namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Resource
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Path { get; set; }
        [Required]
        public ResourceType ResourceType { get; set; }
        [Required]
        public string ResourceTypeId { get; set; }
    }
}
