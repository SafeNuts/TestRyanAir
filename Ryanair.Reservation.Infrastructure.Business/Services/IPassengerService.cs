using Ryanair.Reservation.Infrastructure.Business.Domain;
using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Business.Services
{
    public interface IPassengerService
    {
        IEnumerable<Passenger> GetPassengersByFlightKey(string flightKey);

        void CreatePassengers(string flightKey, IEnumerable<Passenger> passengers);
    }
}
