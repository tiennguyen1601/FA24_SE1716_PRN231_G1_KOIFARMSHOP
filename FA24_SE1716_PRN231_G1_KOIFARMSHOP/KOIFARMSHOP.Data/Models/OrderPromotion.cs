﻿using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class OrderPromotion
{
    public int OrderPromotionId { get; set; }

    public int? PromotionId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Promotion? Promotion { get; set; }
}
