namespace DesignPatterns.Visitor;

public sealed record BookingDetail(string Description, string SpecificInfo);

public sealed class UseCases(DateValidator _validator)
{
    public IEnumerable<BookingDetail> GetBookingDetails(IEnumerable<IBooking> bookings)
    {
        foreach (var booking in bookings)
        {
            yield return booking switch
            {
                HotelBooking hotelBooking => new BookingDetail(
                    $"Hotel Booking for {hotelBooking.HotelName} on {hotelBooking.CheckInDate.ToShortDateString()}",
                    $"Breakfast included: {hotelBooking.IncludesBreakfast}"
                ),
                FlightBooking flightBooking => new BookingDetail(
                    $"Flight Booking {flightBooking.FlightNumber} departing on {flightBooking.DepartureTime}",
                    $"Seat Class: {flightBooking.SeatClass}"
                ),
                _ => throw new NotImplementedException()
            };
        }
    }

    public async Task<bool> ValidateAsync(IEnumerable<IBooking> bookings, CancellationToken ct)
    {
        IEnumerable<Task<bool>> GetTasks()
        {
            foreach (var booking in bookings)
            {
                yield return booking switch
                {
                    HotelBooking hotelBooking => _validator.ValidateDateAsync(hotelBooking.CheckInDate, ct),
                    FlightBooking flightBooking => _validator.ValidateDateAsync(flightBooking.DepartureTime, ct),
                    _ => Task.FromResult(true)
                };
            }
        }

        var validationResults = await Task.WhenAll(GetTasks());
        return validationResults.All(x => x);
    }
}
