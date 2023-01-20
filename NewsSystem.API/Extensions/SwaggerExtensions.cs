using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace NewsSystem.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
    {
        return services.AddSwaggerGen(config =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            config.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
            config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {jwtSecurityScheme, Array.Empty<string>()}
            });
        });
    }
}