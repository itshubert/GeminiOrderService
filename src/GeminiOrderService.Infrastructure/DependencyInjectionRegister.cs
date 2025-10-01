using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Infrastructure.Interceptors;
using GeminiOrderService.Infrastructure.Models;
using GeminiOrderService.Infrastructure.Persistence;
using GeminiOrderService.Infrastructure.Persistence.Repositories;
using GeminiOrderService.Infrastructure.Services.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

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

        services.AddGeminiServices(configuration);

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICatalogService, CatalogService>();

        return services;
    }

    private static IServiceCollection AddGeminiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {


        services.Configure<ServicesSettings>(configuration.GetSection("Services"));

        Random jitterer = new();

        var servicesSettings = new ServicesSettings();
        configuration.GetSection("Services").Bind(servicesSettings);

        SetupHttpClient<CatalogServiceClient>(
            services,
            servicesSettings.CatalogServiceBaseUrl,
            jitterer);

        return services;
    }

    private static void SetupHttpClient<T>(
        IServiceCollection services,
        string baseUrl,
        Random jitterer) where T : class
    {
        services.AddHttpClient<T>((serviceProvider, client) =>
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException($"Base URL cannot be null or empty for type {typeof(T).Name}");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddTransientHttpErrorPolicy(policy =>
            policy.Or<TimeoutRejectedException>()
        .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(T));
            logger.LogWarning($"{nameof(T)} RETRY {retryAttempt} AFTER {timespan.TotalSeconds} seconds");
        }))
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3)));
    }
}