using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class ProductImage
{
    public int ProductId { get; set; }

    public int Id { get; set; }

    public string Image { get; set; } = null!;
}
