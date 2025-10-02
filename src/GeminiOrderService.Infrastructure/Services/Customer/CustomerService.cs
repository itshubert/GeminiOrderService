using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Infrastructure.Services.Customer.Contracts;

namespace GeminiOrderService.Infrastructure.Services.Customer;

public sealed class CustomerService(
    CustomerServiceClient customerServiceClient) : ICustomerService
{
    public async Task<Guid> CreateCustomerAsync(
        string firstName,
        string lastName,
        string email,
        CancellationToken cancellationToken)
    {
        var request = new CreateCustomerRequest(
            firstName,
            lastName,
            email);

        var customerId = await customerServiceClient.CreateCustomerAsync(request, cancellationToken);

        return customerId;
    }
}