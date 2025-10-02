namespace GeminiOrderService.Contracts.Orders;

public sealed record ShippingAddressRequest(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country);
