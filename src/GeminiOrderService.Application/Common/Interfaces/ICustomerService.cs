namespace GeminiOrderService.Application.Common.Interfaces;

public interface ICustomerService
{
    Task<Guid> CreateCustomerAsync(
        string FirstName,
        string LastName,
        string Email,
        CancellationToken cancellationToken);
}