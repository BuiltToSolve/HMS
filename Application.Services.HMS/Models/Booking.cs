using System;
using System.Collections.Generic;
using Application.Services.HMS.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RoomId { get; set; }
    public Room Room { get; set; }

    [Required, MaxLength(50)]
    public string BookingReference { get; set; }

    public Guid? UserId { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public int GuestCount { get; set; } = 1;
    public int RoomCount { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    [Required, MaxLength(255)]
    public string GuestName { get; set; }

    [Required, MaxLength(20)]
    public string GuestPhone { get; set; }

    [Required, MaxLength(255)]
    public string GuestEmail { get; set; }

    [MaxLength(255)]
    public string? GstCompanyName { get; set; }

    [MaxLength(50)]
    public string? GstNumber { get; set; }

    public string? GstAddress { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    public bool Deleted { get; set; } = false;

    public ICollection<PantryOrder> PantryOrders { get; set; }
}