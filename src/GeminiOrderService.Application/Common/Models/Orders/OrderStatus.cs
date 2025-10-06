namespace GeminiOrderService.Application.Common.Models.Orders;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}