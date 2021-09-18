namespace DeemZ.Services.InformativeMessageServices
{
    using System.Collections.Generic;

    public interface IInformativeMessageService
    {
        IEnumerable<T> GetInformativeMessages<T>();
    }
}