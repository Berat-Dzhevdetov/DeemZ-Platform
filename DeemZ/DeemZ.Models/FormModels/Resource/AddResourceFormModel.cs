namespace DeemZ.Models.FormModels.Resource
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.Resource;

    public class AddResourceFormModel
    {
        [Required]
        [StringLength(MaxNameLength,
            ErrorMessage = "{0} of resource should be between {2} and {1} letters",
            MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Display(Name = "Resource type")]
        public string ResourceTypeId { get; set; }

        [StringLength(10000,
            ErrorMessage = "{0} of resource path should be between {2} and {1} letters",
            MinimumLength = PathMinLength)]
        [Required]
        public string Path { get; set; }

        public IEnumerable<ResourceTypeFormModel> ResourceTypes { set; get; } = new HashSet<ResourceTypeFormModel>();
    }
}