using System;
using System.Collections.Generic;
using Application.Services.HMS.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RoomPriceModifier
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    [Required, MaxLength(50)]
    public string RateName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public PriceModifierType PriceType { get; set; } = PriceModifierType.Percentage;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Value { get; set; } = 0;

    public int Priority { get; set; } = 0;
}