using MediatR;
using Microsoft.AspNetCore.Identity;
using NewsSystem.Domain;

namespace NewsSystem.Application.Command;

public record RegisterCommand(string UserName, string Password) : IRequest<IdentityResult>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly IIdentityRepository _identityRepository;

    public RegisterCommandHandler(IIdentityRepository identityRepository)
    {
        _identityRepository = identityRepository;
    }

    public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _identityRepository.FindByName(request.UserName) is not null)
        {
            var identityError = new IdentityError() {Description = "User has existed"};
            return IdentityResult.Failed(identityError);
        }

        var user = new User(request.UserName);
        var result = await _identityRepository.Create(user, request.Password);
        if (!result.Succeeded)
        {
            return result;
        }

        return await _identityRepository.AddToRole(user, "user");
    }
}