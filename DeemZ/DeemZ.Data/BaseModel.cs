namespace DeemZ.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class BaseModel
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}