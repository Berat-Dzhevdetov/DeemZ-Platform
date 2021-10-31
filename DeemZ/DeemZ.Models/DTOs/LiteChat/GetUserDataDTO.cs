using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.DTOs.LiteChat
{
    public class GetUserDataDTO
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationUserUsername { get; set; }
        public string ApplicationUserImgUrl { get; set; }
    }
}
