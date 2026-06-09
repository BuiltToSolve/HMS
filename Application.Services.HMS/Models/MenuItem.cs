using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MenuItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [MaxLength(50)]
    public string? Category { get; set; }

    public bool IsVeg { get; set; } = true;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool Deleted { get; set; } = false;

    public ICollection<PantryOrderItem> PantryOrderItems { get; set; }
}