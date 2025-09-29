namespace GeminiOrderService.Contracts.Orders;

public sealed record CreateOrderRequest(
    Guid CustomerId,
    string Currency,
    IEnumerable<CreateOrderItemRequest> Items);