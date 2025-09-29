using GeminiOrderService.Application.Common.Models.Products;

namespace GeminiOrderService.Application.Common.Interfaces;

public interface ICatalogService
{
    Task<bool> ValidateProductExistsAsync(Guid productId, CancellationToken cancellationToken);
    Task<ProductSummaryModel?> GetProductInfoAsync(Guid productId, CancellationToken cancellationToken);
}