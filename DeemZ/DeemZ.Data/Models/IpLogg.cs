namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.IpLogger;
    public class IpLogg : BaseModel
    {
        [Required]
        [MaxLength(MaxIpLength)]
        public string Ip { get; set; }
        public City City { get; set; }
        public string CityId { get; set; }
    }
}
  