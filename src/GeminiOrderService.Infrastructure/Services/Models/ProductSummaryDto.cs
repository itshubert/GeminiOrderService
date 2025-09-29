namespace GeminiOrderService.Infrastructure.Services.Models;

internal sealed record ProductSummaryDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsActive);