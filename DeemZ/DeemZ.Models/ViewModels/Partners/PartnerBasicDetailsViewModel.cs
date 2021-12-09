namespace DeemZ.Models.ViewModels.Partners
{
    using DeemZ.Data;

    public class PartnerBasicDetailsViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public PartnerTiers Tier { get; set; }
        public string Url { get; set; }
    }
}
