using System.Net.Http.Json;
using GeminiOrderService.Application.Common.Models.Products;
using GeminiOrderService.Infrastructure.Services.Catalog.Contracts;

namespace GeminiOrderService.Infrastructure.Services.Catalog;

public sealed class CatalogServiceClient
{
    private readonly HttpClient _httpClient;

    public CatalogServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductSummaryModel?> GetProductSummaryAsync(Guid productId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/{productId}/summary", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var product = await response.Content.ReadFromJsonAsync<ProductSummaryResponse>(cancellationToken: cancellationToken);
            if (product == null)
            {
                throw new Exception("Failed to deserialize product data.");
            }

            return new ProductSummaryModel(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.IsActive);
        }
        catch (Exception ex)
        {
            // Log the exception (omitted for brevity)
            throw new Exception($"Error fetching product summary for ProductId: {productId}", ex);
        }


    }
}