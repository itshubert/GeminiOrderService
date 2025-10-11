namespace GeminiOrderService.Application.Common.Models.Orders;

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