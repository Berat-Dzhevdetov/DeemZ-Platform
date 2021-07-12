namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<CreateForumTopicFormModel, Forum>();
        }
    }
}
