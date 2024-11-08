﻿using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Brand { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Discount { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public DateOnly? ManufacturingDate { get; set; }

    public int? CategoryId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Staff? CreatedByNavigation { get; set; }

    public virtual Staff? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
