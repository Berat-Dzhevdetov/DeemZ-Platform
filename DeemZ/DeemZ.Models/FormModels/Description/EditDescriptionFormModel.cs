namespace DeemZ.Models.FormModels.Description
{
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.Description;
    public class EditDescriptionFormModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(
            MaxNameLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinNameLength)]
        public string Name { get; set; }
    }
}
