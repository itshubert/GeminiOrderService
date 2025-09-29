namespace GeminiOrderService.Contracts.Orders;

public sealed record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice);