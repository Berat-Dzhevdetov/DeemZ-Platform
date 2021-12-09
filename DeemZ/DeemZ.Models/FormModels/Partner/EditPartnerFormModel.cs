namespace DeemZ.Models.FormModels.Partner
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;

    using static DeemZ.Data.DataConstants.Partners;

    public class EditPartnerFormModel
    {
        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; }
        public IFormFile LogoImage { set; get; }
        public string Path { get; set; }
        [Required]
        public PartnerTiers Tier { get; set; }
        public IDictionary<int, string> Tiers { get; set; }
        [Url]
        public string Url { get; set; }
        public bool IsImageChanged { get; set; }
    }
}
