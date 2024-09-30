using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? DiscountPercent { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderPromotion> OrderPromotions { get; set; } = new List<OrderPromotion>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
