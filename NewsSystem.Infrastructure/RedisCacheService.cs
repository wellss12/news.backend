using System.Text.Json;
using NewsSystem.Application;
using StackExchange.Redis;

namespace NewsSystem.Infrastructure;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<object> Get(string key)
    {
        var value = await _database.StringGetAsync(key);
        return string.IsNullOrWhiteSpace(value)
            ? default
            : JsonSerializer.Deserialize<object>(value);
    }

    public Task Set(string key, string value, TimeSpan expiredTime)
    {
        return _database.StringSetAsync(key, value, expiredTime);
    }

    public Task<bool> SAdd(string key, string field)
    {
        return _database.SetAddAsync(key, field);
    }

    public async Task<string[]> SMembers(string key)
    {
        var members = await _database.SetMembersAsync(key);
        return members.Select(member => (string) member!).ToArray();
    }
}