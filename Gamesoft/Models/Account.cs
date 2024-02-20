using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte GroupId { get; set; }
}
