using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class AnimalImage
{
    public int AimageId { get; set; }

    public int? AnimalId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual Animal? Animal { get; set; }
}
