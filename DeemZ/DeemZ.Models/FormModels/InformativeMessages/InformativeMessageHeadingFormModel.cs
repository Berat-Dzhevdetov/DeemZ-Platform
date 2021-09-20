namespace DeemZ.Models.FormModels.InformativeMessages
{
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.InformativeMessages;

    public class InformativeMessageHeadingFormModel
    {
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
        public string Title { get; set; }
    }
}