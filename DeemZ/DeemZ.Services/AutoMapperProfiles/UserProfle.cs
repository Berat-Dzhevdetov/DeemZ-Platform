namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.User;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, BasicUserInformationViewModel>();
        }
    }
}
