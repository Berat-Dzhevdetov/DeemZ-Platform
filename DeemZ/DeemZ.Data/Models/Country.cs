namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Country : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}