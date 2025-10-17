using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models.Orders;
using MapsterMapper;
using MediatR;

namespace GeminiOrderService.Application.Orders.Queries;

public sealed record GetOrdersByCustomerQuery(
    Guid CustomerId,
    CancellationToken CancellationToken = default) : IRequest<IEnumerable<OrderModel>>;

public sealed class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderModel>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderModel>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderModel>>(orders);
    }
}