using AutoMapper;
using DeemZ.Data.Models;
using DeemZ.Models.ViewModels.ChatMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Services.AutoMapperProfiles
{
    public class ChatMessagesProfile : Profile
    {
        public ChatMessagesProfile()
        {
            CreateMap<ChatMessage, ChatMessageViewModel>();
        }
    }
}
