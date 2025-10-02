using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GeminiOrderService.Infrastructure.Persistence;

public class GeminiOrderDbContextFactory : IDesignTimeDbContextFactory<GeminiOrderDbContext>
{
    public GeminiOrderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GeminiOrderDbContext>();

        // Use a connection string for design-time only
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=geminiorder;Username=postgres;Password=Blackbox57!");

        return new GeminiOrderDbContext(optionsBuilder.Options, null!);
    }
}
