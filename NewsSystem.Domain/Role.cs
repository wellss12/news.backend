using Microsoft.AspNetCore.Identity;

namespace NewsSystem.Domain;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
        Id = Guid.NewGuid();
    }
}