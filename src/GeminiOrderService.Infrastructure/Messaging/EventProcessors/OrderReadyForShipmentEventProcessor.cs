using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class OrderReadyForShipmentEventProcessor : IEventProcessor<OrderReadyForShipmentEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderReadyForShipmentEventProcessor> _logger;

    public OrderReadyForShipmentEventProcessor(IMediator mediator, ILogger<OrderReadyForShipmentEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(OrderReadyForShipmentEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderReadyForShipmentEvent: {OrderId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateOrderStatusCommand(@event.OrderId, Application.Common.Models.Orders.OrderStatus.ReadyForShipment), cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating tracking number for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        _logger.LogInformation("Successfully updated tracking number for OrderId {OrderId}", @event.OrderId);

        return true;
    }
}

