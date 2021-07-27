namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Resource;

    public class ResourceType : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool NeedFileUploadInput { get; set; }

        [Required]
        [MaxLength(MaxIconLength)]
        public string Icon { get; set; }
    }
}