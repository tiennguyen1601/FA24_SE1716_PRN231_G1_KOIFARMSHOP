using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.OrderDTO
{
    public class OrderFilterReqModel
    {
        public List<string>? ShippingAddress { get; set; }
        public List<string>? DeliveryMethod { get; set; }

        public List<string>? PaymentStatus { get; set; }
        public decimal? TotalAmountVAT { get; set; }
        public string? SearchValue { get; set; }
    }

    
}
