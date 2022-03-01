namespace DeemZ.Models.DTOs.IpLogg
{
    using System.ComponentModel.DataAnnotations;
    using static DeemZ.Data.DataConstants.IpLogger;

    public class IpLoggDTO
    {
        [Required]
        [MaxLength(MaxIpLength)]
        public string Ip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country_Name { get; set; }

    }
}
