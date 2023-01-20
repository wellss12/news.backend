using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsSystem.Application;
using NewsSystem.Infrastructure.Config;

namespace NewsSystem.Infrastructure;

public class TokenService : ITokenService
{
    private readonly JWTOptions _options;

    public TokenService(IOptions<JWTOptions> options)
    {
        _options = options.Value;
    }

    public string BuildToken(IEnumerable<Claim> claims)
    {
        var expirationTime = GetExpirationTime(_options.ExpireSeconds);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_options.Issuer,
            _options.Audience,
            claims,
            expires: expirationTime,
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private static DateTime GetExpirationTime(int expireSeconds)
    {
        var now = DateTime.Now;
        return now.Add(TimeSpan.FromSeconds(expireSeconds));
    }
}