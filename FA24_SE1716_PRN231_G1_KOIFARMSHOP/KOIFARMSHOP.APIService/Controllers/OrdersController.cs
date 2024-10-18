using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet]
        public async Task<IBusinessResult> GetProducts(int? page = 1, int? size = 10)
        {
            return await _orderService.GetAll(page, size);
        }

        // GET: api/Orders
        //[HttpGet]
        //public async Task<IBusinessResult> GetOrders()
        //{
        //    return await _orderService.GetAll();
        //}

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetOrder(int id)
        {
            return await _orderService.GetByID(id);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutOrder([FromBody] OrderCompleteRequest orderCompleteRequest)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            return await _orderService.Save(orderCompleteRequest, token);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IBusinessResult> PostOrder([FromBody] OrderCompleteRequest orderCompleteRequest)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            return await _orderService.Save(orderCompleteRequest, token);
        }


        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderResult = await _orderService.GetByID(id);
            if (orderResult == null || orderResult.Data == null)
            {
                return NotFound();
            }

            await _orderService.DeleteByID(id);
            return NoContent();
        }
    }
}
