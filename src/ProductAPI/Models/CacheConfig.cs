using System;
using Microsoft.Extensions.Caching.Memory;

namespace ProductAPI.Models
{
    public class CacheConfig
    {
        public bool Enabled { get; set; }
        
        public TimeSpan Period { get; set; }

        public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions
        {
            Size = 1,
            AbsoluteExpirationRelativeToNow = Period
        };
    }
}