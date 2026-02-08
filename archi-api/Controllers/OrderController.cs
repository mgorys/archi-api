using Archi.Models.OrderModel;
using Archi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Archi.Controllers
{
    [ApiController]
    [Route("api/order")]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var brands = await _orderService.GetAllAsync();

            return Ok(brands);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrderAsync([FromBody] CreateOrderDto dto)
        {
            var id = await _orderService.CreateOrderAsync(dto);
            return Created($"/api/order/{id}", null);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetAsync([FromRoute] int id)
        {
            var phone = await _orderService.GetByIdAsync(id);

            return Ok(phone);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateOrderDto dto, [FromRoute] int id)
        {
            await _orderService.UpdateOrderAsync(id, dto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
