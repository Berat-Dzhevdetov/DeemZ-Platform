namespace DeemZ.Services.InformativeMessageServices
{
    using System.Collections.Generic;

    public interface IInformativeMessageService
    {
        IEnumerable<T> GetInformativeMessages<T>();
        IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20);
        void CreateInformativeMessagesHeading(string title);

        // imhId => InformativeMessagesHeadingsId
        T GetInformativeMessagesHeading<T>(string imhId);
        void EditInformativeMessagesHeading(string imhId, string title);
        void DeleteInformativeMessagesHeading(string imhId);
        void DeleteInformativeMessage(string imId);
        T GetInformativeMessage<T>(string imId);
    }
}