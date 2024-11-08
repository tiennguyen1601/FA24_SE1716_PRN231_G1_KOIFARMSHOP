﻿using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Animal
{
    public int AnimalId { get; set; }

    public string? Name { get; set; }

    public string? Origin { get; set; }

    public string? Species { get; set; }

    public string? Type { get; set; }

    public string? Gender { get; set; }

    public string? Size { get; set; }

    public string? Certificate { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? MaintenanceCost { get; set; }

    public string? Color { get; set; }

    public decimal? AmountFeed { get; set; }

    public string? HealthStatus { get; set; }

    public string? FarmOrigin { get; set; }

    public int? BirthYear { get; set; }

    public string? Description { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual ICollection<AnimalImage> AnimalImages { get; set; } = new List<AnimalImage>();

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual Staff? CreatedByNavigation { get; set; }

    public virtual Staff? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
