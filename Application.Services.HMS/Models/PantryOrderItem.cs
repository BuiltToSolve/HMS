using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PantryOrderItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PantryOrderId { get; set; }
    public PantryOrder? PantryOrder { get; set; }

    public Guid MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PriceAtTime { get; set; }

    public bool Deleted { get; set; } = false;
}