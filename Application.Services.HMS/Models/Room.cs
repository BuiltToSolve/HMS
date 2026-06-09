using System;
using System.Collections.Generic;
using Application.Services.HMS.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Room
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required]
    public RoomType Type { get; set; } = RoomType.Standard;

    [Column(TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; } = 0;

    public int RoomCount { get; set; } = 0;

    public int? AvailableCount { get; set; } = 0;

    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Tag { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public bool Deleted { get; set; } = false;

    // Navigation Properties
    public ICollection<RoomPriceModifier> PriceModifiers { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Amenity> Amenities { get; set; }
    public ICollection<RoomGallery> Gallery { get; set; }
    public ICollection<Booking> Bookings { get; set; }

    [NotMapped]
    public int ReviewCount => Reviews?.Count ?? 0;
}