namespace GeminiOrderService.Contracts.Orders;

public sealed record CreateOrderRequest(
    Guid? CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string Currency,
    ShippingAddressRequest ShippingAddress,
    IEnumerable<CreateOrderItemRequest> Items);