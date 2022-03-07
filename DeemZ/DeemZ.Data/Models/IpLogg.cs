namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.IpLogger;
    public class IpLogg : BaseModel
    {
        [Required]
        [MaxLength(MaxIpLength)]
        public string Ip { get; set; }
        public City City { get; set; }
        public string CityId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
  