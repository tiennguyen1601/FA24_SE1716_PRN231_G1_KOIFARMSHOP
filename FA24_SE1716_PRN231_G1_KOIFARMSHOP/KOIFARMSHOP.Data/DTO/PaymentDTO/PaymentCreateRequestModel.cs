using KOIFARMSHOP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.PaymentDTO
{
    public class PaymentCreateRequestModel
    {
        public int PaymentId { get; set; }

        public int? OrderId { get; set; }

        public string? Method { get; set; }

        public string? Status { get; set; }

        public string? TransactionId { get; set; }
    }
}
