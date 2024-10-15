using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.DTO.OrderDTO
{
    public class OrderRequestModel
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public int? PromotionId { get; set; }
        public string? ShippingAddress { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal? Vat { get; set; }
        public decimal? TotalAmountVat { get; set; }
        public string? Status { get; set; }
    }
    public class OrderDetailRequest
    {
        public int? ProductID { get; set; }
        public int? AnimalID { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Discount { get; set; }

    }
}