using KOIFARMSHOP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTOs
{
    public class OrderBuyRequestModel
    {
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }

   
        public string? ShippingAddress { get; set; }

        public string? DeliveryMethod { get; set; }

        public decimal? Vat { get; set; }

        public decimal? TotalAmountVat { get; set; }

        public string? Status { get; set; }

        public virtual Promotion? Promotion { get; set; }
    }
}
