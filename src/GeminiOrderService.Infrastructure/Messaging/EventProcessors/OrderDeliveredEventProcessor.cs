using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class OrderDeliveredEventProcessor : IEventProcessor<OrderDeliveredEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderDeliveredEventProcessor> _logger;

    public OrderDeliveredEventProcessor(IMediator mediator, ILogger<OrderDeliveredEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(OrderDeliveredEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing OrderDeliveredEventProcessor: {EventId}", @event.OrderId);

        var result = await _mediator.Send(new UpdateOrderStatusCommand(@event.OrderId, OrderStatus.Delivered), cancellationToken);

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