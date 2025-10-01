namespace GeminiOrderService.Infrastructure.Services.Catalog.Contracts;

internal sealed record ProductSummaryResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsActive);