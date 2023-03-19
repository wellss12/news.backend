using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using NewsSystem.Domain;

namespace NewsSystem.Application.Command;

public record LoginCommand(string UserName, string Password) : IRequest<(SignInResult, string?)>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, (SignInResult, string?)>
{
    private readonly IIdentityRepository _identityRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(ITokenService tokenService, IIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
        _tokenService = tokenService;
    }

    public async Task<(SignInResult, string?)> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _identityRepository.FindByName(command.UserName);
        if (user is null)
        {
            return (SignInResult.Failed, null);
        }

        if (!await _identityRepository.CheckForPassword(user, command.Password))
        {
            return (SignInResult.Failed, null);
        }

        var claims = await CreateClaims(user);

        return (SignInResult.Success, _tokenService.BuildToken(claims));
    }

    private async Task<IEnumerable<Claim>> CreateClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var roles = await _identityRepository.GetRoles(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        claims.AddRange(roleClaims);
        return claims;
    }
}