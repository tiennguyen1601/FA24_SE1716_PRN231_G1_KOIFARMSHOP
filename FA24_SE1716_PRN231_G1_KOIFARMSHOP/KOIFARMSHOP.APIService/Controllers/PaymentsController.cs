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
using KOIFARMSHOP.Data.DTO.PaymentDTO;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IBusinessResult> GetPayments()
        {
            return await _paymentService.GetAll();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetPayment(int id)
        {
            return await _paymentService.GetById(id);
        }

        // PUT: api/Payments/5
        [HttpPut]
        public async Task<IBusinessResult> PutPayment(PaymentCreateRequestModel payment)
        {
            return await _paymentService.Save(payment);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IBusinessResult> PostPayment(PaymentCreateRequestModel payment)
        {
            return await _paymentService.Save(payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeletePayment(int id)
        {
            return await _paymentService.DeleteById(id);
        }
    }
}
