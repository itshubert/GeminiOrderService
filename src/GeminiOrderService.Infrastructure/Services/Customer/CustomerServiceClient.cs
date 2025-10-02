using System.Net.Http.Json;
using GeminiOrderService.Infrastructure.Services.Customer.Contracts;

namespace GeminiOrderService.Infrastructure.Services.Customer;

public sealed class CustomerServiceClient
{
    private readonly HttpClient _httpClient;

    public CustomerServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/customers", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdCustomer = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>(cancellationToken: cancellationToken);
            if (createdCustomer == null)
            {
                throw new Exception("Failed to deserialize created customer data.");
            }

            return createdCustomer.Customer.Id;
        }
        catch (Exception ex)
        {
            // Log the exception (omitted for brevity)
            throw new Exception("Error creating customer", ex);
        }
    }
}