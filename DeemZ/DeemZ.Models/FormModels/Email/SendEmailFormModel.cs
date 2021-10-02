using DeemZ.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.FormModels.Email
{
    public class SendEmailFormModel
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public IEnumerable<BasicUserInformationViewModel> Users { get; set; }

        public string[] SelectedUsers { get; set; }
    }
}
