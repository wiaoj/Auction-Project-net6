using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Queries;
using Orders.Application.Responses;
using System.Net;

namespace ESourcing.Order.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase {
    private readonly IMediator _mediator;
    //private readonly ILogger _logger;

    public OrdersController(IMediator mediator/*, ILogger logger*/) {
        this._mediator = mediator;
        //this._logger = logger;
    }

    [HttpGet("[action]/{username}")]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (Int32)HttpStatusCode.OK)]
    [ProducesResponseType((Int32)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(String username) {
        var query = new GetOrdersBySellerUsernameQuery(username);
        var orders = await _mediator.Send(query);

        if(orders.Count().Equals(Decimal.Zero))
            return NotFound();

        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (Int32)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command) {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
