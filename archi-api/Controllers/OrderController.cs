using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Phones.Controllers
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
        public ActionResult<IEnumerable<OrderDto>> GetAll()
        {
            var brands = _orderService.GetAll();

            return Ok(brands);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateOrder([FromBody] CreateOrderDto dto)
        {
            var id = _orderService.CreateOrder(dto);
            return Created($"/api/order/{id}", null);
        }
       
        [HttpGet("{id}")]
        public ActionResult<OrderDto> Get([FromRoute] int id)
        {
            var phone = _orderService.GetById(id);

            return Ok(phone);
        }
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateOrderDto dto, [FromRoute] int id)
        {
            _orderService.UpdateOrder(id, dto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _orderService.DeleteBrand(id);
            return NoContent();
        }
    }
}
