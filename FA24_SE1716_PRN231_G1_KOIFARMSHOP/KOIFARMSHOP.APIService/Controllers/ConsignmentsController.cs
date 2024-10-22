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
using NuGet.Common;
using KOIFARMSHOP.Data.DTO.ConsignmentDTO;

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

        [HttpGet]
        public async Task<IBusinessResult> GetConsignments()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            return await _consignmentService.GetAll(token);
        }

        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetConsignment(int id)
        {
            var consignment = await _consignmentService.GetByID(id);  

            return consignment;
        }

        
        [HttpPut]
        public async Task<IBusinessResult> PutConsignment(Consignment consignment)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            return await _consignmentService.Save(consignment, token);
        }

       
        [HttpPost]
        public async Task<IBusinessResult> PostConsignment(CreateConsignmentReq consignment)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            Consignment newConsignment = new Consignment
            {
                AnimalId = consignment.AnimalId,
                ConsignmentType = consignment.ConsignmentType,
                StartDate = consignment.StartDate,
                EndDate = consignment.EndDate,
                Price = consignment.Price,
                Notes = consignment.Notes,
                CommissionRate = consignment.CommissionRate
            };

            return await _consignmentService.Save(newConsignment, token);
        }

       
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
