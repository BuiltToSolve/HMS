using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Services.HMS.Database;
using System.Linq;

namespace Application.Services.HMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnumController : ControllerBase
{
    [HttpGet("roomtypes")]
    public IActionResult GetRoomTypes()
    {
        var values = Enum.GetValues(typeof(RoomType))
            .Cast<RoomType>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }

    [HttpGet("bookingstatus")]
    public IActionResult GetBookingStatus()
    {
        var values = Enum.GetValues(typeof(BookingStatus))
            .Cast<BookingStatus>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }

    [HttpGet("gallerycategories")]
    public IActionResult GetGalleryCategories()
    {
        var values = Enum.GetValues(typeof(GalleryCategory))
            .Cast<GalleryCategory>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }

    [HttpGet("pantryorderstatus")]
    public IActionResult GetPantryOrderStatus()
    {
        var values = Enum.GetValues(typeof(PantryOrderStatus))
            .Cast<PantryOrderStatus>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }

    [HttpGet("paymentstatus")]
    public IActionResult GetPaymentStatus()
    {
        var values = Enum.GetValues(typeof(PaymentStatus))
            .Cast<PaymentStatus>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }
    
    [HttpGet("pricemodifiertypes")]
    public IActionResult GetPriceModifierTypes()
    {
        var values = Enum.GetValues(typeof(PriceModifierType))
            .Cast<PriceModifierType>()
            .Select(e => new { Id = (int)e, Name = e.ToString() });

        return Ok(values);
    }
    
}
