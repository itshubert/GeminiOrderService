namespace GeminiOrderService.Domain.Orders;

public enum OrderStatus
{
    Pending,
    Confirmed,
    InProgress,
    ReadyForShipment,
    Shipped,
    Delivered,
    Cancelled
}