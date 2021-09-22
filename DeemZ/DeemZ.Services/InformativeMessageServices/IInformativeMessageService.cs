namespace DeemZ.Services.InformativeMessageServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.InformativeMessages;

    public interface IInformativeMessageService
    {
        // imhId => InformativeMessagesHeadingsId
        IEnumerable<T> GetInformativeMessagesCurrentlyInShow<T>();
        IEnumerable<T> GetInformativeMessages<T>(string imhId, int page = 1, int quantity = 20);
        IEnumerable<T> GetInformativeMessageHeadings<T>();
        bool HeadingExits(string imhId);
        IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20);
        void CreateInformativeMessagesHeading(string title);
        T GetInformativeMessagesHeading<T>(string imhId);
        void EditInformativeMessagesHeading(string imhId, string title);
        void DeleteInformativeMessagesHeading(string imhId);
        string DeleteInformativeMessage(string imId);
        T GetInformativeMessage<T>(string imId);
        void CreateInformativeMessage<T>(string imhId, T message);
        string EditInformativeMessage(string imId, InformativeMessageEditFormModel message);
    }
}