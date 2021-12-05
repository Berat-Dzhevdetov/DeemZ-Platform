namespace DeemZ.Models.ViewModels.Partners
{
    using System;
    using DeemZ.Data;

    public class PartnersDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public PartnerTiers Tier { get; set; }
        public DateTime From { get; set; }
    }
}
