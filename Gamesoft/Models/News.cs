using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public DateOnly CreationDate { get; set; }
}
