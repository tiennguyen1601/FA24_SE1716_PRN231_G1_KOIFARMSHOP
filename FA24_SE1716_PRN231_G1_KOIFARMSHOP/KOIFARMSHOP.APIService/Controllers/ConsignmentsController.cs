using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;

namespace KoiFarmShop.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentsController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly IConsignmentService _consignmentService;
        public ConsignmentsController(IConsignmentService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        // GET: api/Consignments
        [HttpGet]
        public async Task<IBusinessResult> GetConsignments()
        {
            return await _consignmentService.GetAll();
        }

        // GET: api/Consignments/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetConsignment(int id)
        {
            var consignment = await _consignmentService.GetByID(id);  

            return consignment;
        }

        // PUT: api/Consignments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IBusinessResult> PutConsignment(Consignment consignment)
        {
            return await _consignmentService.Save(consignment);
        }

        // POST: api/Consignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostConsignment(Consignment consignment)
        {
            return await _consignmentService.Save(consignment);
        }

        // DELETE: api/Consignments/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteConsignment(int id)
        {
            return await _consignmentService.DeleteByID(id);
        }

        private bool ConsignmentExists(int id)
        {
            return _consignmentService.GetByID(id) != null;
        }
    }
}
