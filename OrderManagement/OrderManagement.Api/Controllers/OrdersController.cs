using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.CreateOrder;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(new { OrderId = orderId });
        }
    }
}
