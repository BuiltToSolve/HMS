using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    [MaxLength(50)]
    public string? GuestName { get; set; }

    [Column(TypeName = "decimal(3,2)")]
    public decimal? Rating { get; set; }

    [MaxLength(500)]
    public string? ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }
}