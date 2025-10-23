using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GeminiOrderService.Infrastructure.Persistence;

public class GeminiOrdersDbContextFactory : IDesignTimeDbContextFactory<GeminiOrderDbContext>
{
    public GeminiOrderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GeminiOrderDbContext>();

        // Use a connection string for design-time only
        optionsBuilder.UseNpgsql("Host=localhost;Port=6543;Database=geminiorder;Username=hubertd;Password=Blackbox57!");

        return new GeminiOrderDbContext(optionsBuilder.Options, null!);
    }
}
