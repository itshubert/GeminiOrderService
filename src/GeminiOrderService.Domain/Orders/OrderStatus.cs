namespace GeminiOrderService.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Confirmed,
    InProgress,
    Shipped,
    Delivered,
    Cancelled
}