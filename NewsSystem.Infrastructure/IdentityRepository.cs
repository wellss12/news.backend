using Microsoft.AspNetCore.Identity;
using NewsSystem.Domain;

namespace NewsSystem.Infrastructure;

public class IdentityRepository : IIdentityRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public IdentityRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public Task<User?> FindByName(string userName)
    {
        return _userManager.FindByNameAsync(userName)!;
    }

    public async Task<bool> CheckForPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public Task<IdentityResult> Create(User user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddToRole(User user, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var role = new Role() {Name = roleName};
            var identityResult = await _roleManager.CreateAsync(role);
            if (!identityResult.Succeeded)
            {
                return identityResult;
            }
        }

        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IList<string>> GetRoles(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}