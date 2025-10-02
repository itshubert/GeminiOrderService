namespace GeminiOrderService.Infrastructure.Services.Customer.Contracts;

public sealed record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email);