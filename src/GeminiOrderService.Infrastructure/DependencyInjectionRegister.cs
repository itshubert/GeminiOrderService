using GeminiOrderService.Infrastructure.Interceptors;
using GeminiOrderService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiOrderService.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GeminiOrderDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("GeminiOrderDbContext");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }
}