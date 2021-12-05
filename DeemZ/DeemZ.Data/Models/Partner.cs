namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Partners;

    public class Partner : BaseModel
    {
        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }
        [Required]
        public string PublicId { get; set; }
        [Required]
        public string Path { get; set; }
        public PartnerTiers Tier { get; set; }
        public DateTime From { get; set; }
    }
}
