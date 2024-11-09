using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? AnimalId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Discount { get; set; }

    public virtual Animal? Animal { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
