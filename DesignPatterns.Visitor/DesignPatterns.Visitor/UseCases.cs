namespace DesignPatterns.Visitor;

public sealed record BookingDetail(string Description, string SpecificInfo);

public sealed class UseCases(DateValidator _validator)
{
    public IEnumerable<BookingDetail> GetBookingDetails(IEnumerable<IBooking> bookings)
    {
        foreach (var booking in bookings)
        {
            if (booking is HotelBooking hotelBooking)
            {
                yield return new BookingDetail(
                    $"Hotel Booking for {hotelBooking.HotelName} on {hotelBooking.CheckInDate.ToShortDateString()}",
                    $"Breakfast included: {hotelBooking.IncludesBreakfast}"
                );
            }
            else if (booking is FlightBooking flightBooking)
            {
                yield return new BookingDetail(
                    $"Flight Booking {flightBooking.FlightNumber} departing on {flightBooking.DepartureTime}",
                    $"Seat Class: {flightBooking.SeatClass}"
                );
            }
        }
    }

    public async Task<bool> ValidateAsync(IEnumerable<IBooking> bookings, CancellationToken ct)
    {
        IEnumerable<Task<bool>> GetTasks()
        {
            foreach (var booking in bookings)
            {
                if (booking is HotelBooking hotelBooking)
                {
                    yield return _validator.ValidateDateAsync(hotelBooking.CheckInDate, ct);
                }
                else if (booking is FlightBooking flightBooking)
                {
                    yield return _validator.ValidateDateAsync(flightBooking.DepartureTime, ct);
                }
            }
        }

        var validationResults = await Task.WhenAll(GetTasks());
        return validationResults.All(x => x);
    }
}
