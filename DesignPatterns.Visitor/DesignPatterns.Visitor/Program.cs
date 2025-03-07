﻿using DesignPatterns.Visitor;
using Microsoft.Extensions.DependencyInjection;

var app = new ServiceCollection()
    .AddTransient<DateValidator>()
    .AddTransient<UseCases>()
    .BuildServiceProvider();

var useCases = app.GetRequiredService<UseCases>();

List<IBooking> bookings = 
[
    new HotelBooking("Hotel California", DateTime.Today.AddDays(10), true),
    new FlightBooking("Flight 200", DateTime.Now.AddDays(5), "Economy"),
];

var status = await useCases.ValidateAsync(bookings, CancellationToken.None);
Console.WriteLine($"Validation status is: {status}");

foreach (var detail in useCases.GetBookingDetails(bookings))
{
    Console.WriteLine(detail);
}
