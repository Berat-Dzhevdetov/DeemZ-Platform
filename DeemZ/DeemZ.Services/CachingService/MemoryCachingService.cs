namespace DeemZ.Services.CachingService
{
    using System;
    using Microsoft.Extensions.Caching.Memory;

    public class MemoryCachingService : IMemoryCachingService
    {
        private readonly IMemoryCache memoryCache;
        public MemoryCachingService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T CreateItem<T>(string key, T value, TimeSpan? timeSpan)
        {
            if (!memoryCache.TryGetValue<T>(key, out var foundValue))
            {
                foundValue = value;

                timeSpan = timeSpan == null ? TimeSpan.FromMinutes(30) : timeSpan;

                var memoryChacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration((TimeSpan)timeSpan);

                memoryCache.Set(key, foundValue, memoryChacheEntryOptions);
            }
            return foundValue;
        }

        public bool ItemExists<T>(string key, out T value)
        {
            value = memoryCache.Get<T>(key);
            return value != null;
        }
    }
}
