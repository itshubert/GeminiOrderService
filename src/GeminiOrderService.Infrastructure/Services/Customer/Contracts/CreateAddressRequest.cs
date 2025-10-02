namespace GeminiOrderService.Infrastructure.Services.Customer.Contracts;

public sealed record CreateAddressRequest(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country,
    bool IsDefault);