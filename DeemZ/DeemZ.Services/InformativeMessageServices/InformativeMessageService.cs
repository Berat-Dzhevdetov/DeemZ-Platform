namespace DeemZ.Services.InformativeMessageServices
{
    using AutoMapper;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;

    public class InformativeMessageService : IInformativeMessageService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public InformativeMessageService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<T> GetInformativeMessages<T>()
           => context.InformativeMessagesHeadings
                .Include(x => x.InformativeMessages)
                //.Select(x => new
                //{
                //    x.Title,
                //    InformativeMessages = x.InformativeMessages.Where(x => x.ShowFrom.ToLocalTime() <= DateTime.Now && x.ShowTo.ToLocalTime() > DateTime.Now).ToList()
                //})
                .Where(x => x.InformativeMessages.Any(x => x.ShowFrom <= DateTime.Now && x.ShowTo > DateTime.Now))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();
    }
}