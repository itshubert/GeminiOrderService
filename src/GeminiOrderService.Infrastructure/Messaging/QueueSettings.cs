namespace GeminiOrderService.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string InventoryReserved { get; set; } = string.Empty;
    public string OrderStockFailed { get; set; } = string.Empty;
    public string OrderInProgress { get; set; } = string.Empty;
    public string OrderReadyForShipment { get; set; } = string.Empty;
    public string OrderShipped { get; set; } = string.Empty;
}