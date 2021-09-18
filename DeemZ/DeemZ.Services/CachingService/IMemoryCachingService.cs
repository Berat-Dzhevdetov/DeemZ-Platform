namespace DeemZ.Services.CachingService
{
    using System;
    public interface IMemoryCachingService
    {
        bool ItemExists<T>(string key, out T value);
        T CreateItem<T>(string key, T value, TimeSpan? timeSpan);
    }
}
