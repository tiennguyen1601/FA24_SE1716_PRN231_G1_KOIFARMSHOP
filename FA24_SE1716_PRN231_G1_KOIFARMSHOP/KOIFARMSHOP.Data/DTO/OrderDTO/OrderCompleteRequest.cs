using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KOIFARMSHOP.Data.DTO.OrderDTO
{
    public class OrderCompleteRequest
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        public int? PromotionId { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(255, ErrorMessage = "Shipping address cannot be longer than 255 characters.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Delivery method is required.")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        public string PaymentStatus { get; set; }

        public decimal? Vat { get; set; }
        public int? AnimalID { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public decimal? Amount { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Discount { get; set; }
       
    }

}
