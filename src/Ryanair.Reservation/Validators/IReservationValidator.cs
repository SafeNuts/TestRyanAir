using System.Collections.Generic;

namespace Ryanair.Reservation.Validators
{
    public interface IReservationValidator
    {
        IEnumerable<string> ValidateReservation(Infrastructure.Business.Domain.Reservation reservation);
    }
}
