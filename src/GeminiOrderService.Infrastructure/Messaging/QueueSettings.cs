namespace GeminiOrderService.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string InventoryReserved { get; set; } = string.Empty;
    public string OrderStockFailed { get; set; } = string.Empty;
    public string JobInProgress { get; set; } = string.Empty;
    public string LabelGenerated { get; set; } = string.Empty;
    public string OrderShipped { get; set; } = string.Empty;
}