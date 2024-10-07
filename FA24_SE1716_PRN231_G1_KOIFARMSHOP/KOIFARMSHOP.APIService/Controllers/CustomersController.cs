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
    public class CustomersController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IBusinessResult> GetCusomter()
        {
            return await _customerService.GetAll();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetOrder(int id)
        {
            var customer = await _customerService.GetByID(id);

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomer(Customer customer)
        //{
        //    return await _customerService.Save(customer);
        //}

        //// POST: api/Customers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        //{
        //    return await _customerService.Save(customer);
        //}

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var animal = await _customerService.GetByID(id);
            if (animal == null)
            {
                return NotFound();
            }
            await _customerService.DeleteByID(id);

            return NoContent();
        }
        
    }
}
