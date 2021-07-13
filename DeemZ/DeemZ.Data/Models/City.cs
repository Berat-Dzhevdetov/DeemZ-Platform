namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class City : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Area Area { get; set; }

        [Required]
        public string AreaId { get; set; }
    }
}
