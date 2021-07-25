namespace DeemZ.Models.FormModels.Lecture
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Models.FormModels.Description;

    using static DeemZ.Data.DataConstants.Lecture;

    public class AddLectureFormModel
    {
        [Required]
        [StringLength(MaxNameLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinNameLength)]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public IEnumerable<AddDescriptionFormModel> Descriptions { get; set; } = new List<AddDescriptionFormModel>();
    }
}
