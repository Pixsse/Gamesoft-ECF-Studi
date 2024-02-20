using System;
using System.Collections.Generic;

namespace Gamesoft.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string StudioName { get; set; } = null!;

    public int SupportId { get; set; }

    public byte DevelopementPriority { get; set; }

    public int EngineId { get; set; }

    public DateOnly DateCreated { get; set; }

    public DateOnly? LastUpdatedDate { get; set; }

    public DateOnly? EstimatedCreationEndDate { get; set; }

    public decimal MaxBudget { get; set; }

    public int StatusId { get; set; }

    public int GenreId { get; set; }

    public byte NumberOfPlayer { get; set; }

    public decimal Price { get; set; }

    public string FeaturedImage { get; set; }
}
