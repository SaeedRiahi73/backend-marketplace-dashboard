using Microsoft.Extensions.Caching.Memory;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Models.Security;

namespace Task_Infrastructure.Services.Security;

public sealed class UserTokenValidationMemoryCache
    : IUserTokenValidationCache
{
    private const string CacheKeyPrefix = "user-token-validation:";
    private static readonly TimeSpan CacheDuration =
        TimeSpan.FromSeconds(30);

    private readonly IMemoryCache _cache;

    public UserTokenValidationMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool TryGet(
        Guid userId,
        out UserTokenValidationState? state)
    {
        return _cache.TryGetValue(
            CreateCacheKey(userId),
            out state);
    }

    public void Set(
        Guid userId,
        UserTokenValidationState state)
    {
        _cache.Set(
            CreateCacheKey(userId),
            state,
            CacheDuration);
    }

    public void Remove(Guid userId)
    {
        _cache.Remove(CreateCacheKey(userId));
    }

    private static string CreateCacheKey(Guid userId)
    {
        return $"{CacheKeyPrefix}{userId:N}";
    }
}
