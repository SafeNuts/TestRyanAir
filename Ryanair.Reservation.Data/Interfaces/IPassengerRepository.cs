using Ryanair.Reservation.Data.Entities;
using System.Collections.Generic;

namespace Ryanair.Reservation.Data.Interfaces
{
    public interface IPassengerRepository
    {
        IEnumerable<PassengerEntity> GetAll();

        string Create(PassengerEntity passengerEntity);
    }
}
