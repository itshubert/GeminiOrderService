namespace GeminiOrderService.Application.Common.Models.Orders;

public sealed record OrderItemModel(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal);
