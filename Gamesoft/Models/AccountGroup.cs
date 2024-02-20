using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class AccountGroup
{
    public int GroupId { get; set; }

    public string Name { get; set; } = null!;

    public bool Admin { get; set; }

    public bool CommunityManager { get; set; }

    public bool BasicUser { get; set; }

    public bool Productor { get; set; }
}
