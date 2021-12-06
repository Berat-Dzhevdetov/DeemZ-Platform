namespace DeemZ.Models.FormModels.Partner
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.Partners;

    public class AddPartnerFormModel
    {
        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; }
        public IFormFile LogoImage { set; get; }
        [Required]
        public int Tier { get; set; }
        public IDictionary<int, string> Tiers { get; set; }
    }
}
