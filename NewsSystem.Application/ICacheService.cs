namespace NewsSystem.Application;

public interface ICacheService
{
    Task<object> Get(string key);
    Task Set(string key, string value, TimeSpan expiredTime);
    Task<bool> SAdd(string key, string field);
    Task<string[]> SMembers(string key);
}