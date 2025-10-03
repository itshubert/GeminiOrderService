using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Infrastructure.Interceptors;
using GeminiOrderService.Infrastructure.Models;
using GeminiOrderService.Infrastructure.Persistence;
using GeminiOrderService.Infrastructure.Persistence.Repositories;
using GeminiOrderService.Infrastructure.Services.Catalog;
using GeminiOrderService.Infrastructure.Services.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Amazon.EventBridge;
using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Infrastructure.Messaging;

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
        ConfigureEventBridge(services, configuration);
        services.AddScoped<IEventBridgePublisher, EventBridgePublisher>();

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICustomerService, CustomerService>();

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

        SetupHttpClient<CustomerServiceClient>(
            services,
            servicesSettings.CustomerServiceBaseUrl,
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
        .AddStandardResilienceHandler(options =>
        {
            // Configure retry with exponential backoff and jitter
            options.Retry.MaxRetryAttempts = 4;
            options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
            options.Retry.UseJitter = true;
            options.Retry.Delay = TimeSpan.FromSeconds(2);

            // Configure circuit breaker
            options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;

            // Configure timeout
            options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);
            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
        });
    }

    private static void ConfigureEventBridge(IServiceCollection services, IConfiguration configuration)
    {
        var useLocalStack = configuration.GetValue<bool>("AWS:UseLocalStack");

        if (useLocalStack)
        {
            // Configure for LocalStack
            var serviceUrl = configuration["AWS:LocalStack:ServiceUrl"] ?? "http://localhost:4566";

            services.AddAWSService<IAmazonEventBridge>(new Amazon.Extensions.NETCore.Setup.AWSOptions
            {
                DefaultClientConfig =
                {
                    ServiceURL = serviceUrl
                }
            });

            // Log LocalStack configuration
            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<IAmazonEventBridge>>();
                logger.LogInformation("EventBridge configured for LocalStack at {ServiceUrl}", serviceUrl);
                return sp;
            });
        }
        else
        {
            // Configure for AWS
            services.AddAWSService<IAmazonEventBridge>();

            // Log AWS configuration
            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<IAmazonEventBridge>>();
                var region = configuration["AWS:Region"] ?? "us-east-1";
                logger.LogInformation("EventBridge configured for AWS in region {Region}", region);
                return sp;
            });
        }
    }
}