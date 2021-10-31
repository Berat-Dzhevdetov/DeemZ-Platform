namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.DTOs.LiteChat;
    using DeemZ.Models.FormModels.User;
    using DeemZ.Models.ViewModels.User;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, BasicUserInformationViewModel>();
            CreateMap<ApplicationUser, GetUserDataDTO>()
                .ForMember(x => x.ApplicationUserId, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.ApplicationUserImgUrl, y => y.MapFrom(x => x.ImgUrl))
                .ForMember(x => x.ApplicationUserUsername, y => y.MapFrom(x => x.UserName));

            CreateMap<ApplicationUser, EditUserFormModel>()
                .ReverseMap();
        }
    }
}