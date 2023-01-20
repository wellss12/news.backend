using System.Security.Claims;

namespace NewsSystem.Application;

public interface ITokenService
{
    string BuildToken(IEnumerable<Claim> claims);
}