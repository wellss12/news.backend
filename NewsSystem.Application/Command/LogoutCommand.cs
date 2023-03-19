using MediatR;
using Microsoft.AspNetCore.Http;


namespace NewsSystem.Application.Command;

public record LogoutCommand(Guid UserId) : IRequest<Unit>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly ICacheService _cacheService;
    private readonly HttpContext _httpContext;

    public LogoutCommandHandler(ICacheService cacheService, IHttpContextAccessor httpContextAccessor)
    {
        _cacheService = cacheService;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<Unit> Handle(LogoutCommand command, CancellationToken token)
    {
        var jti = _httpContext.User.FindFirst("jti").Value;
        await _cacheService.SAdd("jwt_black_list", jti);
        return Unit.Value;
    }
}