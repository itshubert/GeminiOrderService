using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class InventoryReservedEventProcessor : IEventProcessor<InventoryReservedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryReservedEventProcessor> _logger;

    public InventoryReservedEventProcessor(IMediator mediator, ILogger<InventoryReservedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ProcessEventAsync(InventoryReservedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing InventoryReservedEvent: {EventId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateOrderStatusCommand(@event.OrderId, OrderStatus.Confirmed), cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }
        }
        else
        {
            _logger.LogInformation("Successfully updated order status for OrderId {OrderId}", @event.OrderId);
        }
    }
}