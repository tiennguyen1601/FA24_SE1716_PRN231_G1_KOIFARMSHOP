using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.PaymentDTO
{
    public class PaymentSearchModel
    {
        public string? Method { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderId { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
