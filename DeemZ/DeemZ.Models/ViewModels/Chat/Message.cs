using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.ViewModels.Chat
{
    public class Message
    {
        public string User { get; set; }

        public string Text { get; set; }

        public string CreatedOn { get; set; }
    }
}
