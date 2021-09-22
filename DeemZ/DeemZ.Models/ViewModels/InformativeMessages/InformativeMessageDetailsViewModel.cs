namespace DeemZ.Models.ViewModels.InformativeMessages
{
    using System;

    public class InformativeMessageDetailsViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime ShowFrom { get; set; } = DateTime.UtcNow;
        public DateTime ShowTo { get; set; } = DateTime.UtcNow.AddDays(7);
    }
}