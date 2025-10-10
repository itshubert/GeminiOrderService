using GeminiOrderService.Api.Common.Mappings;
using GeminiOrderService.Application.Common.Interfaces;
using GeminiOrderService.Application.Orders.Queries;
using GeminiOrderService.Contracts;
using GeminiOrderService.Contracts.Orders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiOrderService.Api.Controllers;

[Route("[controller]")]
public sealed class OrdersController : BaseController
{
    private readonly ICustomerService _customerService;

    public OrdersController(
        ISender mediator,
        IMapper mapper,
        ICustomerService customerService) : base(mediator, mapper)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOrdersQuery(
            pageNumber,
            pageSize,
            cancellationToken);

        var response = await Mediator.Send(query, cancellationToken);
        var items = Mapper.Map<IEnumerable<OrderResponse>>(response.Items);

        return Ok(new PagedResultResponse<OrderResponse>(
            items,
            response.TotalCount,
            response.PageNumber,
            response.PageSize));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        Guid customerId;

        if (!IsUserAuthenticated() || request.CustomerId is null)
        {
            // create customer record
            customerId = await _customerService.CreateCustomerAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                cancellationToken);
        }
        else
        {
            customerId = request.CustomerId.Value;
        }

        // var command = new CreateOrderCommand(
        //     customerId,
        //     request.Currency,
        //     Mapper.Map<ShippingAddressCommand>(request.ShippingAddress),
        //     request.Items.Select(i => new CreateOrderItemCommand(
        //         i.ProductId,
        //         i.Quantity,
        //         i.UnitPrice
        //     )));

        var command = request.ToCreateOrderCommand();

        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            order => CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order),
            Problem);
    }

}