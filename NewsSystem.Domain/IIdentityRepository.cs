using Microsoft.AspNetCore.Identity;

namespace NewsSystem.Domain;

public interface IIdentityRepository
{
    Task<User?> FindByName(string userName);
    Task<bool> CheckForPassword(User user, string password);
    Task<IdentityResult> Create(User user, string password);
    Task<IdentityResult> AddToRole(User user, string roleName);
    Task<IList<string>> GetRoles(User user);
}