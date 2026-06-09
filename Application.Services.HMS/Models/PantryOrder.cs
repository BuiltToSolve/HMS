using System;
using System.Collections.Generic;
using Application.Services.HMS.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PantryOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? BookingId { get; set; }
    public Booking? Booking { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public PantryOrderStatus Status { get; set; } = PantryOrderStatus.Received;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public bool Deleted { get; set; } = false;

    public ICollection<PantryOrderItem> Items { get; set; }
}