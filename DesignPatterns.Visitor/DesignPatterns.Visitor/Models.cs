namespace DesignPatterns.Visitor;

public interface IBookingVisitor<T>
{
    T Visit(HotelBooking hotelBooking);
    T Visit(FlightBooking flightBooking);
    T Visit(CarRentalBooking carRentalBooking);
}

public sealed record MatchBooking<T>(
    Func<HotelBooking, T> TransformHotelBooking,
    Func<FlightBooking, T> TransformFlightBooking,
    Func<CarRentalBooking, T> TransformCarRentalBooking
) : IBookingVisitor<T>
{
    public T Visit(HotelBooking hotelBooking) => TransformHotelBooking(hotelBooking);
    public T Visit(FlightBooking flightBooking) => TransformFlightBooking(flightBooking);
    public T Visit(CarRentalBooking carRentalBooking) => TransformCarRentalBooking(carRentalBooking);
}

public interface IBooking
{
    public T Accept<T>(IBookingVisitor<T> visitor);
}

public sealed record HotelBooking(string HotelName, DateTime CheckInDate, bool IncludesBreakfast) : IBooking
{
    public T Accept<T>(IBookingVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record FlightBooking(string FlightNumber, DateTime DepartureTime, string SeatClass) : IBooking
{
    public T Accept<T>(IBookingVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record CarRentalBooking(string RentalCompany, string CarModel, DateTime PickupDate, bool IncludesInsurance) : IBooking
{
    public T Accept<T>(IBookingVisitor<T> visitor) => visitor.Visit(this);
}
