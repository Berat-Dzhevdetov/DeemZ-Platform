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

            CreateMap<Forum, TopicViewModel>()
                .ForMember(x => x.UserProfileImg, o => o.MapFrom(src => src.User.ImgUrl))
                .ForMember(x => x.Username, o => o.MapFrom(src => src.User.UserName))
                .ForMember(x => x.Comments, o => o.MapFrom(src => src.Comments));

            CreateMap<Comment, TopicMainComments>()
                .ForMember(x => x.UserProfileImg, o => o.MapFrom(src => src.Forum.User.ImgUrl))
                .ForMember(x => x.Username, o => o.MapFrom(src => src.Forum.User.UserName))
                .ForMember(x => x.Answers, o => o.MapFrom(src => src.АnswerТоId));

            CreateMap<AddMainCommentFormModel, Comment>();
        }
    }
}