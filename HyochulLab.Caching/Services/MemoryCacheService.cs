using Microsoft.Extensions.Caching.Memory;
using HyochulLab.Caching.Interfaces;

namespace HyochulLab.Caching.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? data);
        return Task.FromResult(data);
    }

    public Task SetAsync<T>(string key, T data, TimeSpan cacheDuration)
    {
        _cache.Set(key, data, cacheDuration);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
