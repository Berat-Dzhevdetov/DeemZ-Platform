namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InformativeMessage : BaseModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime ShowFrom { get; set; } = DateTime.Now;
        public DateTime ShowTo { get; set; } = DateTime.Now.AddDays(7);

    }
}