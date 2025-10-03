using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Domain.Orders.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Application.Orders.Events;

public sealed record OrderSubmittedEventHandler : INotificationHandler<OrderSubmitted>
{
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly ILogger<OrderSubmittedEventHandler> _logger;

    public OrderSubmittedEventHandler(
        IEventBridgePublisher eventBridgePublisher,
        ILogger<OrderSubmittedEventHandler> logger)
    {
        _eventBridgePublisher = eventBridgePublisher;
        _logger = logger;
    }

    public async Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
    {
        var order = new
        {
            OrderId = notification.Order.Id,
            notification.Order.CustomerId,
            notification.Order.OrderDate,
            notification.Order.TotalAmount,
            Items = notification.OrderItems.Select(item => new
            {
                item.ProductId,
                item.Quantity,
                item.UnitPrice
            })
        };

        await _eventBridgePublisher.PublishAsync("OrderSubmitted", order, cancellationToken);
    }
}