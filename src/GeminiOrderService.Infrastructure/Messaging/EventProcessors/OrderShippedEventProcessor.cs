using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class OrderShippedEventProcessor : IEventProcessor<OrderShippedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderShippedEventProcessor> _logger;

    public OrderShippedEventProcessor(IMediator mediator, ILogger<OrderShippedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task<bool> ProcessEventAsync(OrderShippedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderShippedEvent: {OrderId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateOrderStatusCommand(@event.OrderId, Application.Common.Models.Orders.OrderStatus.Shipped), cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating order status for OrderId {OrderId}: {Error}", @event.OrderId, error);
            }

            return false;
        }

        _logger.LogInformation("Successfully updated order status for OrderId {OrderId}", @event.OrderId);

        return true;

    }
}