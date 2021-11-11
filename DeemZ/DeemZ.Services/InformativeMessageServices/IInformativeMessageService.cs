namespace DeemZ.Services.InformativeMessageServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.InformativeMessages;

    public interface IInformativeMessageService
    {
        // imhId => InformativeMessagesHeadingsId
        IEnumerable<T> GetInformativeMessagesCurrentlyInShow<T>();
        IEnumerable<T> GetInformativeMessages<T>(string imhId, int page = 1, int quantity = 20);
        IEnumerable<T> GetInformativeMessageHeadings<T>();
        bool HeadingExits(string imhId);
        IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20);
        Task CreateInformativeMessagesHeading(string title);
        T GetInformativeMessagesHeading<T>(string imhId);
        Task EditInformativeMessagesHeading(string imhId, string title);
        Task DeleteInformativeMessagesHeading(string imhId);
        Task<string> DeleteInformativeMessage(string imId);
        T GetInformativeMessage<T>(string imId);
        Task CreateInformativeMessage<T>(string imhId, T message);
        Task<string> EditInformativeMessage(string imId, InformativeMessageEditFormModel message);
    }
}