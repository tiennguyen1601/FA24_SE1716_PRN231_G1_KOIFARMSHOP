﻿using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Consignment
{
    public int ConsignmentId { get; set; }

    public int? CustomerId { get; set; }

    public int? AnimalId { get; set; }

    public string? ConsignmentType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Notes { get; set; }

    public decimal? CommissionRate { get; set; }

    public int? OrderId { get; set; }

    public virtual Animal? Animal { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Order? Order { get; set; }
}
