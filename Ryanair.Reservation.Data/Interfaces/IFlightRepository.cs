using Ryanair.Reservation.Data.Entities;
using System.Collections.Generic;

namespace Ryanair.Reservation.Data.Interfaces
{
    public interface IFlightRepository
    {
        IEnumerable<FlightEntity> GetAll();
        FlightEntity Get(string key);
    }
}
