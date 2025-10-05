using GeminiOrderService.Domain.Orders;

namespace GeminiOrderService.Application.Common.Models.Orders;

public sealed record OrderModel(
    Guid Id,
    Guid CustomerId,
    DateTimeOffset OrderDate,
    string Status,
    decimal TotalAmount,
    string Currency,
    IEnumerable<OrderItemModel> Items);