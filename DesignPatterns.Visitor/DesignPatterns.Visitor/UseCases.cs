namespace DesignPatterns.Visitor;

public sealed record BookingDetail(string Description, string SpecificInfo);

public sealed class UseCases(DateValidator _validator)
{
    public IEnumerable<BookingDetail> GetBookingDetails(IEnumerable<IBooking> bookings)
    {
        foreach (var booking in bookings)
        {
            yield return booking.Accept(GetBookingDetailsVisitor.Instance);
        }
    }

    private sealed class GetBookingDetailsVisitor : IBookingVisitor<BookingDetail>
    {
        public static GetBookingDetailsVisitor Instance { get; } = new();

        public BookingDetail Visit(HotelBooking hotelBooking) => new(
            $"Hotel Booking for {hotelBooking.HotelName} on {hotelBooking.CheckInDate.ToShortDateString()}",
            $"Breakfast included: {hotelBooking.IncludesBreakfast}"
        );

        public BookingDetail Visit(FlightBooking flightBooking) => new(
            $"Flight Booking {flightBooking.FlightNumber} departing on {flightBooking.DepartureTime}",
            $"Seat Class: {flightBooking.SeatClass}"
        );

        public BookingDetail Visit(CarRentalBooking carRentalBooking) => new(
            $"Car Rental from {carRentalBooking.RentalCompany} on {carRentalBooking.PickupDate.ToShortDateString()}",
            $"Insurance included: {carRentalBooking.IncludesInsurance}"
        );
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
