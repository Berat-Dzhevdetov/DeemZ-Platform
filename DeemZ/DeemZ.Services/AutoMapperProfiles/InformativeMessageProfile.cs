namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.InformativeMessages;
    using DeemZ.Models.ViewModels.InformativeMessages;

    public class InformativeMessageProfile : Profile
    {
        public InformativeMessageProfile()
        {
            CreateMap<InformativeMessagesHeading, InformativeMessagesHeadingViewModel>();

            CreateMap<InformativeMessage,InformativeMessageViewModel>();

            CreateMap<InformativeMessagesHeading, InformativeMessageHeadingFormModel>();
        }
    }
}