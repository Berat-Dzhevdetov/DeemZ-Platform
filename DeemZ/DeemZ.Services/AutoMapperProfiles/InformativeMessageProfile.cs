namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.InformativeMessages;

    public class InformativeMessageProfile : Profile
    {
        public InformativeMessageProfile()
        {
            CreateMap<InformativeMessagesHeading, InformativeMessagesHeadingViewModel>();

            CreateMap<InformativeMessage,InformativeMessageViewModel>();
        }
    }
}