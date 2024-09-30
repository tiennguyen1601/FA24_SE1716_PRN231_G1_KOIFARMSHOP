using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class ProductImage
{
    public int PimageId { get; set; }

    public int? ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
