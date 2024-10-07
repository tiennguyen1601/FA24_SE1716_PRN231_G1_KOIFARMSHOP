using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Services;
using KOIFARMSHOP.Service.Base;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
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
            var order = await _orderService.GetByID(id);

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutOrder(Order order)
        {
            return await _orderService.Save(order);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostOrder(Order order)
        {
            return await _orderService.Save(order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var animal = await _orderService.GetByID(id);
            if (animal == null)
            {
                return NotFound();
            }
            await _orderService.DeleteByID(id);

            return NoContent();
        }
    }
}
