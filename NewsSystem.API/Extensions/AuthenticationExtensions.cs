using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NewsSystem.Application;
using NewsSystem.Infrastructure.Config;

namespace NewsSystem.API.Extensions;

public static class AuthenticationExtensions
{
    public static void AddJWTAuthentication(this IServiceCollection services, JWTOptions jwtOptions)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = async context =>
                    {
                        var jti = context.Principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
                        //TODO: clear black_list regular
                        if ((await cacheService.SMembers("jwt_black_list")).Contains(jti))
                        {
                            context.Fail("token fail");
                        }
                    }
                };
            });
        services.AddAuthorization();
    }
}