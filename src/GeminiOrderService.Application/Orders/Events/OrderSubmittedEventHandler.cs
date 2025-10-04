using GeminiOrderService.Application.Common.Messaging;
using GeminiOrderService.Application.Common.Models.Orders;
using GeminiOrderService.Domain.Orders.Events;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeminiOrderService.Application.Orders.Events;

public sealed record OrderSubmittedEventHandler : INotificationHandler<OrderSubmitted>
{
    private readonly IEventBridgePublisher _eventBridgePublisher;
    private readonly ILogger<OrderSubmittedEventHandler> _logger;
    private readonly IMapper _mapper;

    public OrderSubmittedEventHandler(
        IEventBridgePublisher eventBridgePublisher,
        ILogger<OrderSubmittedEventHandler> logger,
        IMapper mapper)
    {
        _eventBridgePublisher = eventBridgePublisher;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<OrderModel>(notification.Order);

        await _eventBridgePublisher.PublishAsync("OrderSubmitted", order, cancellationToken);
    }
}