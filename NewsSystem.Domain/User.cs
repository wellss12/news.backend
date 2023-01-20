using Microsoft.AspNetCore.Identity;

namespace NewsSystem.Domain;

public class User : IdentityUser<Guid>
{
    public User(string userName) : base(userName)
    {
        Id = Guid.NewGuid();
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset CreatedTime { get; init; }
}