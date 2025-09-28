namespace GeminiOrderService.Contracts.Orders;

public sealed record OrderResponse(
    Guid Id,
    DateTime OrderDate,
    string CustomerName,
    decimal TotalAmount,
    IEnumerable<OrderItemResponse> Items
);