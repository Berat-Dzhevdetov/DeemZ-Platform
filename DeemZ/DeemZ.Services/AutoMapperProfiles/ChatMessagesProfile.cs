namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.ChatMessages;
    public class ChatMessagesProfile : Profile
    {
        public ChatMessagesProfile()
        {
            CreateMap<ChatMessage, ChatMessageViewModel>();
        }
    }
}