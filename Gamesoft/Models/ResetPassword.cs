using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class ResetPassword
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }
}
