using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class OrderStockFailedEventProcessor : IEventProcessor<OrderStockFailedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderStockFailedEventProcessor> _logger;

    public OrderStockFailedEventProcessor(IMediator mediator, ILogger<OrderStockFailedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ProcessEventAsync(OrderStockFailedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderStockFailedEvent: {EventId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateOrderStatusCommand(@event.OrderId, OrderStatus.Cancelled), cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error cancelling order for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }
        }
        else
        {
            _logger.LogInformation("Successfully cancelled order for OrderId {OrderId}", @event.OrderId);
        }

        // TODO: Create failed orders table for auditing purposes
        foreach (var item in @event.FailedItems)
        {
            _logger.LogWarning("OrderId {OrderId} - ProductId {ProductId} failed due to: {Reason}", @event.OrderId, item.ProductId, item.Reason);
        }

    }
}