using System;
using System.Collections.Generic;

namespace Ryanair.Reservation.Validators.Implementations
{
    public class FlightSearchRequestValidator : IFlightSearchRequestValidator
    {
        public IEnumerable<string> ValidateFlightSearchRequest(int passengers, string origin, string destination, DateTime dateOut, DateTime? dateIn, bool? roundTrip)
        {
            var errorMessages = new List<string>();

            if (passengers < 0 || passengers > 50)
            {
                errorMessages.Add("Please, provide number of passengers between 0 and 50.");
            }

            if (string.IsNullOrWhiteSpace(origin))
            {
                errorMessages.Add("Please, provide origin of the flight.");
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                errorMessages.Add("Please, provide destination of the flight.");
            }

            if (dateOut == new DateTime())
            {
                errorMessages.Add("Please, provide valid date out.");
            }

            if (roundTrip == null || !roundTrip.Value)
            {
                return errorMessages;
            }

            if (dateIn == null || dateIn.Value < dateOut)
            {
                errorMessages.Add("Please, provide valid date in.");
            }

            return errorMessages;
        }
    }
}
