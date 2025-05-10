using static DesignPatterns.Visitor.UseCases;

namespace DesignPatterns.Visitor;

public sealed record BookingDetail(string Description, string SpecificInfo);

internal sealed class UseCases(ValidateAsyncVisitor _validateAsyncVisitor)
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
                yield return booking.Accept(_validateAsyncVisitor).Invoke(ct);
            }
        }

        var validationResults = await Task.WhenAll(GetTasks());
        return validationResults.All(x => x);
    }

    internal sealed class ValidateAsyncVisitor(DateValidator _validator) : IBookingVisitor<Func<CancellationToken, Task<bool>>>
    {
        public Func<CancellationToken, Task<bool>> Visit(HotelBooking hotelBooking)
        {
            return ct => _validator.ValidateDateAsync(hotelBooking.CheckInDate, ct);
        }

        public Func<CancellationToken, Task<bool>> Visit(FlightBooking flightBooking)
        {
            return ct => _validator.ValidateDateAsync(flightBooking.DepartureTime, ct);
        }

        public Func<CancellationToken, Task<bool>> Visit(CarRentalBooking carRentalBooking)
        {
            return ct => _validator.ValidateDateAsync(carRentalBooking.PickupDate, ct);
        }
    }
}
