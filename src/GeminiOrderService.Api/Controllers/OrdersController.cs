using GeminiOrderService.Application.Orders.Queries;
using GeminiOrderService.Contracts.Orders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiOrderService.Api.Controllers;

[Route("[controller]")]
public sealed class OrdersController : ApiController
{

    public OrdersController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var query = new GetAllOrdersQuery();
        var response = await Mediator.Send(query, cancellationToken);

        return Ok(response);
    }

}