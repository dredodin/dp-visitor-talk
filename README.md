This repository serves as an example of how the `Visitor` pattern can be implemented. Each branch represents a step in the refactoring process.

## 01-init (main)
- Add `IBooking` with implementations `HotelBooking` and `FlightBooking`
- Add `UseCases`: `GetBookingDetails`, `ValidateAsync` using `is` pattern matching
## 02-new-implementation
- Add `CarRentalBooking` but forget of usecase
## 03-switch-pattern-matching
- Convert `is` to `switch`
## 04-visitor
- Temp comment `CarRentalBooking`
- Create `IBookingVisitor` and add method `Accept`
- Convert `GetBookingDetails` into a visitor approach 
## 05-new-implementation-v2
- Add `CarRentalBooking` no way to forget of usecase
## 06-visitor-with-state
- Convert `ValidateAsync` into a visitor with state approach 
## 07-visitor-with-currying
- - Convert `ValidateAsync` into a visitor with currying approach
## 08-universal-visitor
- Add `MatchBooking` visitor
- Add `getDate` example
## 09-compare-with-switch
- Add `getDate` logic via switch expression and compare
