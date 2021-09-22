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

            CreateMap<InformativeMessage, InformativeMessageDetailsViewModel>();

            CreateMap<InformativeMessageFormModel, InformativeMessage>();

            CreateMap<InformativeMessage, InformativeMessageEditFormModel>()
                .ForMember(x => x.ShowFrom, o => o.MapFrom(src => src.ShowFrom.ToLocalTime()))
                .ForMember(x => x.ShowTo, o => o.MapFrom(src => src.ShowTo.ToLocalTime()))
                .ReverseMap();
        }
    }
}