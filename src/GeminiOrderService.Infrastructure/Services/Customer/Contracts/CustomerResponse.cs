namespace GeminiOrderService.Infrastructure.Services.Customer.Contracts;

public sealed record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);