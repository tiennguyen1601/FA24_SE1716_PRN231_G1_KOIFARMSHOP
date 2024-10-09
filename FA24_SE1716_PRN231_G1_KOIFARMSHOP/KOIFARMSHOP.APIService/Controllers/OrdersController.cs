using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Service.Services;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.OrderDTO.KOIFARMSHOP.Data.DTO.OrderDTO;

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

        // GET: api/Orders
        [HttpGet]
        public async Task<IBusinessResult> GetOrders()
        {
            return await _orderService.GetAll();
        }

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
            return await _orderService.Save(orderCompleteRequest.Order, orderCompleteRequest.OrderDetails);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IBusinessResult> PostOrder([FromBody] OrderCompleteRequest orderCompleteRequest)
        {
            return await _orderService.Save(orderCompleteRequest.Order, orderCompleteRequest.OrderDetails);
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
