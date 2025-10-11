using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Orders.Commands;
using GeminiOrderService.Infrastructure.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Infrastructure.Messaging.EventProcessors;

public sealed class LabelGeneratedEventProcessor : IEventProcessor<LabelGeneratedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<LabelGeneratedEventProcessor> _logger;

    public LabelGeneratedEventProcessor(IMediator mediator, ILogger<LabelGeneratedEventProcessor> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> ProcessEventAsync(LabelGeneratedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing LabelGeneratedEvent: {OrderId}", @event.OrderId);

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