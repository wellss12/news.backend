using Microsoft.AspNetCore.Identity;
using NewsSystem.Domain;
using NewsSystem.Infrastructure;

namespace NewsSystem.API.Extensions;

public static class IdentityExtensions
{
    public static void ConfigureIdentity(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddDataProtection();
        webApplicationBuilder.Services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        });
        var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), webApplicationBuilder.Services);
        idBuilder.AddEntityFrameworkStores<IdDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>();
    }
}