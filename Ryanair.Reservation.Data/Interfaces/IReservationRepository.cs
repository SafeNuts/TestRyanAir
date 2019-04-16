using Ryanair.Reservation.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ryanair.Reservation.Data.Interfaces
{
    public interface IReservationRepository
    {
        string Create(ReservationEntity item);
        ReservationEntity Get(string key);
        IEnumerable<ReservationEntity> GetAll();
        bool CheckIfKeyExists(string key);
    }
}
