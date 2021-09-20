namespace DeemZ.Services.InformativeMessageServices
{
    using AutoMapper;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using DeemZ.Global.Extensions;
    using DeemZ.Data.Models;

    public class InformativeMessageService : IInformativeMessageService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public InformativeMessageService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void CreateInformativeMessagesHeading(string title)
        {
            context.InformativeMessagesHeadings.Add(new InformativeMessagesHeading
            {
                Title = title
            });

            context.SaveChanges();
        }

        public IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20)
            => context.InformativeMessagesHeadings
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantiy)
                .ToList();


        public IEnumerable<T> GetInformativeMessages<T>()
           => context.InformativeMessagesHeadings
                .Include(x => x.InformativeMessages)
                .Where(x => x.InformativeMessages.Any(x => x.ShowFrom <= DateTime.Now && x.ShowTo > DateTime.Now))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();
    }
}