namespace DesignPatterns.Visitor;

public interface IBooking;

public sealed record HotelBooking(string HotelName, DateTime CheckInDate, bool IncludesBreakfast) : IBooking;

public sealed record FlightBooking(string FlightNumber, DateTime DepartureTime, string SeatClass) : IBooking;

public sealed record CarRentalBooking(string RentalCompany, string CarModel, DateTime PickupDate, bool IncludesInsurance) : IBooking;