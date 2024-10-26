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

        public int? PromotionId { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(255, ErrorMessage = "Shipping address cannot be longer than 255 characters.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Delivery method is required.")]
        public string DeliveryMethod { get; set; }

        public int? AnimalID { get; set; }
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }

       
    }

}
