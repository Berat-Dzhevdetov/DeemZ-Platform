namespace DeemZ.Services.InformativeMessageServices
{
    using System.Collections.Generic;

    public interface IInformativeMessageService
    {
        IEnumerable<T> GetInformativeMessages<T>();
        IEnumerable<T> GetInformativeMessageHeadings<T>(int page = 1, int quantiy = 20);
        void CreateInformativeMessagesHeading(string title);
    }
}