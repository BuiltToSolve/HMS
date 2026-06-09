using System;
using System.Collections.Generic;
using Application.Services.HMS.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RoomGallery
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RoomId { get; set; }
    public Room Room { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public GalleryCategory Category { get; set; } = GalleryCategory.Room;
}