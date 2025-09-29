using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Common.Models;
using GeminiOrderService.Application.Common.Models.Orders;
using MapsterMapper;
using MediatR;

namespace GeminiOrderService.Application.Orders.Queries;

public sealed record GetOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    CancellationToken CancellationToken = default
) : IRequest<PagedResultModel<OrderModel>>;

public sealed class GetOrdersQueryHandler(
    IOrderRepository orderRepository,
    IMapper mapper
) : IRequestHandler<GetOrdersQuery, PagedResultModel<OrderModel>>
{
    public async Task<PagedResultModel<OrderModel>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var (totalRecords, orders) = await orderRepository.GetOrdersAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken: request.CancellationToken);

        var ordersResponse = mapper.Map<IEnumerable<OrderModel>>(orders);


        return new PagedResultModel<OrderModel>(
            ordersResponse,
            totalRecords,
            request.PageNumber,
            request.PageSize);
    }
}

