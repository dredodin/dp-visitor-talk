namespace DesignPatterns.Visitor;

public interface IBooking;

public sealed record HotelBooking(string HotelName, DateTime CheckInDate, bool IncludesBreakfast) : IBooking;

public sealed record FlightBooking(string FlightNumber, DateTime DepartureTime, string SeatClass) : IBooking;