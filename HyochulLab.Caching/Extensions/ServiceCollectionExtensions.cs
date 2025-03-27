using HyochulLab.Caching.Interfaces;
using HyochulLab.Caching.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HyochulLab.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHyochulLabCaching(this IServiceCollection services)
    {
        services.AddMemoryCache(); // 메모리 캐시 기본 등록
        services.AddSingleton<ICacheService, MemoryCacheService>();

        // Redis 추가 시:
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = "redis-connection-string";
        // });
        // services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
