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
    using DeemZ.Models.FormModels.InformativeMessages;
    using DeemZ.Models.ViewModels.InformativeMessages;

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

        public void DeleteInformativeMessagesHeading(string imhId)
        {
            var imhToDel = context.InformativeMessagesHeadings
                    .Include(x => x.InformativeMessages)
                    .FirstOrDefault(x => x.Id == imhId);

            foreach (var imh in imhToDel.InformativeMessages)
                DeleteInformativeMessage(imh.Id);

            context.InformativeMessagesHeadings.Remove(imhToDel);

            context.SaveChanges();
        }

        public string DeleteInformativeMessage(string imId)
        {
            var msg = GetInformativeMessage<InformativeMessage>(imId);

            context.InformativeMessages.Remove(msg);
            context.SaveChanges();

            return msg.InformativeMessagesHeadingId;
        }

        public T GetInformativeMessage<T>(string imId)
        {
            var msg = context.InformativeMessages.FirstOrDefault(x => x.Id == imId);

            return mapper.Map<T>(msg);
        }

        public void EditInformativeMessagesHeading(string imhId, string title)
        {
            var msgToEdit = GetInformativeMessagesHeading<InformativeMessagesHeading>(imhId);

            context.InformativeMessagesHeadings.Attach(msgToEdit);

            msgToEdit.Title = title;

            context.SaveChanges();
        }

        public IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20)
            => context.InformativeMessagesHeadings
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantiy)
                .ToList();

        public IEnumerable<T> GetInformativeMessageHeadings<T>()
            => context.InformativeMessagesHeadings
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public bool HeadingExits(string imhId)
            => context.InformativeMessagesHeadings
                .Any(x => x.Id == imhId);

        public IEnumerable<T> GetInformativeMessagesCurrentlyInShow<T>()
            => context.InformativeMessagesHeadings
                  .Include(x => x.InformativeMessages)
                  .Select(x => new InformativeMessagesHeadingViewModel() //This can also be done with AutoMapper
                  {
                      Title = x.Title,
                      Id = x.Id,
                      InformativeMessages = x.InformativeMessages
                      .Where(x => x.ShowFrom <= DateTime.Now && x.ShowTo > DateTime.Now)
                      .Select(y => new InformativeMessageViewModel() { Description = y.Description }),
                  })
                  .ProjectTo<T>(mapper.ConfigurationProvider)
                  .ToList();

        //imhId => informativeMessagesHeadingId
        public T GetInformativeMessagesHeading<T>(string imhId)
        {
            var imh = context.InformativeMessagesHeadings
                    .FirstOrDefault(x => x.Id == imhId);

            return mapper.Map<T>(imh);
        }

        public IEnumerable<T> GetInformativeMessages<T>(string imhId, int page = 1, int quantity = 20)
            => context.InformativeMessages
                .Where(x => x.InformativeMessagesHeadingId == imhId)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public void CreateInformativeMessage<T>(string imhId, T message)
        {
            var newlyMessage = mapper.Map<InformativeMessage>(message);

            newlyMessage.ShowFrom = newlyMessage.ShowFrom.ToUniversalTime();
            newlyMessage.ShowTo = newlyMessage.ShowTo.ToUniversalTime();
            newlyMessage.InformativeMessagesHeadingId = imhId;

            context.InformativeMessages.Add(newlyMessage);
            context.SaveChanges();
        }

        public string EditInformativeMessage(string imId, InformativeMessageEditFormModel message)
        {
            var msgToEdit = GetInformativeMessage<InformativeMessage>(imId);

            context.InformativeMessages.Attach(msgToEdit);

            msgToEdit.Description = message.Description;
            msgToEdit.ShowFrom = message.ShowFrom.ToUniversalTime();
            msgToEdit.ShowTo = message.ShowTo.ToUniversalTime();

            context.SaveChanges();

            return msgToEdit.InformativeMessagesHeadingId;
        }
    }
}