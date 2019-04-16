using System;
using System.Collections.Generic;
using System.Linq;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Services;

namespace Ryanair.Reservation.Infrastructure.Business.Validators.Implementations
{
    public class PassengerValidator : IPassengerValidator
    {
        private readonly IPassengerService _passengerService;

        public PassengerValidator(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        public void ValidatePassengers(string flightKey, IEnumerable<Passenger> passengers)
        {
            if (passengers == null)
            {
                throw new ArgumentNullException(nameof(passengers));
            }

            var passengersList = passengers.ToList();

            if (passengersList.Any(x => x.Bags > 5 || x.Bags < 0))
            {
                throw new ArgumentException("Passenger can take only 0 - 5 bags.");
            }

            var passengersOnFlight = _passengerService.GetPassengersByFlightKey(flightKey).ToList();

            var bookedSeats = passengersOnFlight.Where(x => 
                passengersList.Any(c => c.Seat == x.Seat)).Select(x => x.Seat).ToList();

            if (bookedSeats.Any())
            {
                throw new ArgumentException($"One or more seats are already booked. Seats: #{string.Join(", ", bookedSeats)}.");
            }

            var necessaryBagsCount = passengersList.Sum(x => x.Bags);
            var registeredBagsCount = passengersOnFlight.Sum(x => x.Bags);

            var totalBagsCount = necessaryBagsCount + registeredBagsCount;

            if (totalBagsCount > 50)
            {
                throw new ArgumentException($"Unfortunately, there is no place for such number of bags: {totalBagsCount - 50}.");
            }

            if (passengersList.Any(x => !int.TryParse(x.Seat, out int parseResult) || parseResult < 0 || parseResult > 50))
            {
                throw new ArgumentException("Passengers can take seats only between '01' and '50'.");
            }

            var duplicateSeat = passengersList.GroupBy(x => x.Seat).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            if (duplicateSeat.Any())
            {
                throw new ArgumentException($"One or more seats were selected more than one time. Seats: #{string.Join(", ", duplicateSeat)}.");
            }
        }
    }
}
