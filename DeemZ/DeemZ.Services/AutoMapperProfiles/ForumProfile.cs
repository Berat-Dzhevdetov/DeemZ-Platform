namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Models.ViewModels.Forum;

    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<CreateForumTopicFormModel, Forum>();

            CreateMap<Forum, ForumTopicsViewModel>()
                .ForMember(x => x.UserProfileImgUrl, o => o.MapFrom(src => src.User.ImgUrl));

        }
    }
}