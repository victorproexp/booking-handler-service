using Microsoft.AspNetCore.Mvc;
using bookinghandlerAPI.Services;

namespace bookinghandlerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingHandlerController : ControllerBase
{
    private readonly ILogger<BookingHandlerController> _logger;
    private readonly IBookingRepository _repository;

    public BookingHandlerController(ILogger<BookingHandlerController> logger, IBookingRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet()]
    public IEnumerable<Booking> GetBookings()
    {
        return _repository.GetBookings();
    }
}
