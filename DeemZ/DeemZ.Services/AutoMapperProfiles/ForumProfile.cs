namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Models.ViewModels.Forum;
    using System;

    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<CreateForumTopicFormModel, Forum>();

            CreateMap<Forum, ForumTopicsViewModel>()
                .ForMember(x => x.UserProfileImgUrl, o => o.MapFrom(src => src.User.ImgUrl))
                .ForMember(x => x.Username, o => o.MapFrom(src => src.User.UserName));

            CreateMap<Forum, TopicViewModel>()
                .ForMember(x => x.UserProfileImg, o => o.MapFrom(src => src.User.ImgUrl))
                .ForMember(x => x.Username, o => o.MapFrom(src => src.User.UserName))
                .ForMember(x => x.Comments, o => o.MapFrom(src => src.Comments));

            CreateMap<Comment, TopicCommentsViewModel>()
                .ForMember(x => x.UserProfileImg, o => o.MapFrom(src => src.Forum.User.ImgUrl))
                .ForMember(x => x.Username, o => o.MapFrom(src => src.Forum.User.UserName))
                .ForMember(x => x.Description, o => o.MapFrom(src => src.Text));

            CreateMap<AddCommentFormModel, Comment>()
                .ForMember(x => x.CreatedOn, o => o.MapFrom(x => DateTime.UtcNow));

        }
    }
}