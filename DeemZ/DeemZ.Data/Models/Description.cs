namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Description : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}