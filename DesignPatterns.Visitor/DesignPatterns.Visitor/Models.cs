namespace DesignPatterns.Visitor;

public interface IBookingVisitor<T>
{
    T Visit(HotelBooking hotelBooking);
    T Visit(FlightBooking flightBooking);
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

//public sealed record CarRentalBooking(string RentalCompany, string CarModel, DateTime PickupDate, bool IncludesInsurance) : IBooking;