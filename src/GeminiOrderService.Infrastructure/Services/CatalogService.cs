using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Products;
using GeminiOrderService.Infrastructure.Services.Clients;

namespace GeminiOrderService.Infrastructure.Services;

public sealed class CatalogService(
    CatalogServiceClient catalogServiceClient) : ICatalogService
{
    public async Task<bool> ValidateProductExistsAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await catalogServiceClient.GetProductSummaryAsync(productId, cancellationToken);
        return product != null;
    }

    public async Task<ProductSummaryModel?> GetProductInfoAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await catalogServiceClient.GetProductSummaryAsync(productId, cancellationToken);

        return product;
    }
}