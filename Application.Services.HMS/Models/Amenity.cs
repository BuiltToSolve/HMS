using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Amenity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RoomId { get; set; }
    public Room Room { get; set; }

    [MaxLength(250)]
    public string? AmenityName { get; set; }

    [MaxLength(100)]
    public string? Icon { get; set; }
}