namespace bookinghandlerAPI.Services;

public interface IBookingRepository
{
    void Put(Booking booking);
    IEnumerable<Booking> GetBookings();
}

public class BookingRepository : IBookingRepository
{
    public List<Booking> bookings;

    public BookingRepository()
    {
        bookings = new();
    }

    public void Put(Booking booking)
    {
        bookings.Add(booking);
    }

    public IEnumerable<Booking> GetBookings()
    {
        return bookings;
    }
}
