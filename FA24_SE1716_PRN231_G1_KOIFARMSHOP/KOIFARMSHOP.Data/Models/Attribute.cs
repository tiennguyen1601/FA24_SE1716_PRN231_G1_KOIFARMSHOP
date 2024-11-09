using System;
using System.Collections.Generic;

namespace KOIFARMSHOP.Data.Models;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool? IsComparable { get; set; }

    public bool? IsDeletable { get; set; }
}
