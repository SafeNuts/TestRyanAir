using Ryanair.Reservation.Infrastructure.Business.Domain;
using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Business.Validators
{
    public interface IPassengerValidator
    {
        void ValidatePassengers(string flight, IEnumerable<Passenger> passengers); 
    }
}
