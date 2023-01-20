using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NewsSystem.Infrastructure;

public class DesignTimeIdDbContextFactory : IDesignTimeDbContextFactory<IdDbContext>
{
    public IdDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdDbContext>();
        optionsBuilder.UseSqlServer(
            "server=127.0.0.1;uid=sa;pwd=Wells;database=NewsDB;pooling=true;min pool size=50;max pool size=512;Connect Timeout=60;");
        return new IdDbContext(optionsBuilder.Options);
    }
}