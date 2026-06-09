using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    #region DbSets

    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomPriceModifier> RoomPriceModifiers { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RoomGallery> RoomGalleries { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<PantryOrder> PantryOrders { get; set; }
    public DbSet<PantryOrderItem> PantryOrderItems { get; set; }
    public DbSet<CorporateEnquiry> CorporateEnquiries { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Decimal Precision

        modelBuilder.Entity<Room>()
            .Property(x => x.BasePrice)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<RoomPriceModifier>()
            .Property(x => x.Value)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Review>()
            .Property(x => x.Rating)
            .HasColumnType("decimal(3,2)");

        modelBuilder.Entity<Booking>()
            .Property(x => x.TotalAmount)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<MenuItem>()
            .Property(x => x.Price)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<PantryOrder>()
            .Property(x => x.TotalAmount)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<PantryOrderItem>()
            .Property(x => x.PriceAtTime)
            .HasColumnType("decimal(10,2)");

        #endregion

        #region Unique Constraint

        modelBuilder.Entity<Booking>()
            .HasIndex(x => x.BookingReference)
            .IsUnique();

        #endregion

        #region Relationships

        // Room -> Reviews
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reviews)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room -> Amenities
        modelBuilder.Entity<Amenity>()
            .HasOne(a => a.Room)
            .WithMany(r => r.Amenities)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room -> Gallery
        modelBuilder.Entity<RoomGallery>()
            .HasOne(g => g.Room)
            .WithMany(r => r.Gallery)
            .HasForeignKey(g => g.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room -> PriceModifiers
        modelBuilder.Entity<RoomPriceModifier>()
            .HasOne(p => p.Room)
            .WithMany(r => r.PriceModifiers)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room -> Bookings
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Booking -> PantryOrders
        modelBuilder.Entity<PantryOrder>()
            .HasOne(p => p.Booking)
            .WithMany(b => b.PantryOrders)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        // PantryOrder -> PantryOrderItems
        modelBuilder.Entity<PantryOrderItem>()
            .HasOne(i => i.PantryOrder)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.PantryOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // MenuItem -> PantryOrderItems
        modelBuilder.Entity<PantryOrderItem>()
            .HasOne(i => i.MenuItem)
            .WithMany(m => m.PantryOrderItems)
            .HasForeignKey(i => i.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Soft Delete Global Filters

        modelBuilder.Entity<Room>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<Booking>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<MenuItem>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<PantryOrder>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<PantryOrderItem>().HasQueryFilter(x => !x.Deleted);
        modelBuilder.Entity<CorporateEnquiry>().HasQueryFilter(x => !x.Deleted);

        #endregion
    }
}