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
using Microsoft.AspNetCore.Authorization;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IBusinessResult> GetPayments(
                [FromQuery] string? method,
                [FromQuery] string? status,
                [FromQuery] string? transactionId,
                [FromQuery] DateTime? paymentDate,
                [FromQuery] DateTime? createdAt,
                [FromQuery] DateTime? updatedAt,
                [FromQuery] string? customerName,
                [FromQuery] string? orderId,
                [FromQuery] decimal? minAmount,
                [FromQuery] decimal? maxAmount
            )
        {
            var result = await _paymentService.GetAll();
            var list = result.Data as IEnumerable<Payment>;
            if (list == null) return result;

            if (!string.IsNullOrWhiteSpace(method))
            {
                list = list.Where(p => (p.Method ?? "").Contains(method, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                list = list.Where(p => (p.Status ?? "").Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                list = list.Where(p => (p.TransactionId ?? "").Contains(transactionId, StringComparison.OrdinalIgnoreCase));
            }

            if (paymentDate.HasValue)
            {
                list = list.Where(p => p.PaymentDate.HasValue ? p.PaymentDate.Value.Date == paymentDate.Value.Date: false);
            }

            if (createdAt.HasValue)
            {
                list = list.Where(p => p.CreatedAt.HasValue ? p.CreatedAt.Value.Date == createdAt.Value.Date : false);
            }

            if (updatedAt.HasValue)
            {
                list = list.Where(p => p.UpdatedAt.HasValue ? p.UpdatedAt.Value.Date == updatedAt.Value.Date : false);
            }

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                list = list.Where(p => (p.Customer ?? new Customer()).Name.Contains(customerName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(orderId))
            {
                list = list.Where(p => (p.OrderId ?? -1).ToString().Contains(orderId));
            }

            if (minAmount.HasValue)
            {
                list = list.Where(p => (p.Order ?? new Order()).TotalAmountVat >= minAmount);
            }

            if (maxAmount.HasValue)
            {
                list = list.Where(p => (p.Order ?? new Order()).TotalAmountVat <= maxAmount);
            }

            result.Data = list;
            return result;
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
