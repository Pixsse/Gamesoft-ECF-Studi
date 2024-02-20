using System;
using System.Collections.Generic;
using Gamesoft.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamesoft.Contexts;

public partial class GamesoftContext : DbContext
{
    public GamesoftContext()
    {
    }

    public GamesoftContext(DbContextOptions<GamesoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountGroup> AccountGroups { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductEngine> ProductEngines { get; set; }

    public virtual DbSet<ProductFavorite> ProductFavorites { get; set; }

    public virtual DbSet<ProductGenre> ProductGenres { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<ProductSupport> ProductSupports { get; set; }

    public virtual DbSet<ResetPassword> ResetPasswords { get; set; }

    public virtual DbSet<VProductsStore> VProductsStores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "IX_Account_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Account_Pseudo").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.Username).HasMaxLength(25);
        });

        modelBuilder.Entity<AccountGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("Account Group");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasDefaultValue("");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.FeaturedImage)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValue("");
            entity.Property(e => e.MaxBudget).HasColumnType("money");
            entity.Property(e => e.Price)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("money");
            entity.Property(e => e.StudioName)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Gamesoft");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ProductEngine>(entity =>
        {
            entity.HasKey(e => e.EngineId);

            entity.ToTable("Product Engine");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductFavorite>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.AccountId });

            entity.ToTable("Product Favorite");
        });

        modelBuilder.Entity<ProductGenre>(entity =>
        {
            entity.HasKey(e => e.GenreId);

            entity.ToTable("Product Genre");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.Id });

            entity.ToTable("Product Image");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Image).HasMaxLength(250);
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId);

            entity.ToTable("Product Status");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductSupport>(entity =>
        {
            entity.HasKey(e => e.SupportId);

            entity.ToTable("Product Support");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ResetPassword>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.ToTable("ResetPassword");

            entity.Property(e => e.Email).HasMaxLength(80);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(50)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<VProductsStore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vProductsStore");

            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Image)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
