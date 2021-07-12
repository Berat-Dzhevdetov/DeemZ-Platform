namespace DeemZ.Services.FormService
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using DeemZ.Data;
    using System.Linq;

    public class ForumService : IForumService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ForumService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void CreateTopic<T>(string title)
        {
            throw new NotImplementedException();
        }
    }
}
