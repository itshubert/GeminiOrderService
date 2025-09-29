namespace GeminiOrderService.Contracts.Orders;

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    DateTimeOffset OrderDate,
    string Status,
    decimal TotalAmount,
    string Currency,
    IEnumerable<OrderItemResponse> Items
);