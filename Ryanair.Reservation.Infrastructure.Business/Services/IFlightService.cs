using Ryanair.Reservation.Infrastructure.Business.Domain;
using System;
using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Business.Services
{
    public interface IFlightService
    {
        IEnumerable<Flight> SearchAvailableFlights(int passengers, string origin, string destination, DateTime dateOut, DateTime? dateIn, bool roundTrip);

        void CreatePassengersForFlights(IEnumerable<Flight> flights);
    }
}
