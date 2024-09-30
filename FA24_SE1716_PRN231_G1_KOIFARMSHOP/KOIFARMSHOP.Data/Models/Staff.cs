using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public string Role { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Animal> AnimalCreatedByNavigations { get; set; } = new List<Animal>();

    public virtual ICollection<Animal> AnimalModifiedByNavigations { get; set; } = new List<Animal>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductModifiedByNavigations { get; set; } = new List<Product>();
}
