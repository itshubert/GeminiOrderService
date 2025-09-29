namespace GeminiOrderService.Application.Common.Models.Products;

public sealed record ProductSummaryModel(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsActive);