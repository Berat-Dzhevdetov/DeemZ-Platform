namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ResourceType : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}