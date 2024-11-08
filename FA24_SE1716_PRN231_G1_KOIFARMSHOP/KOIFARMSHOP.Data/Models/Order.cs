﻿using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int? PromotionId { get; set; }

    public string? ShippingAddress { get; set; }

    public string? DeliveryMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public decimal? Vat { get; set; }

    public decimal? TotalAmountVat { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderPromotion> OrderPromotions { get; set; } = new List<OrderPromotion>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Promotion? Promotion { get; set; }
}
