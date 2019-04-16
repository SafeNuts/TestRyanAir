using System;
using System.Collections.Generic;

namespace Ryanair.Reservation.Validators
{
    public interface IFlightSearchRequestValidator
    {
        IEnumerable<string> ValidateFlightSearchRequest(int passengers, string origin, string destination, DateTime dateOut,
            DateTime? dateIn, bool? roundTrip);
    }
}
