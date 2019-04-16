using System.Collections.Generic;
using System.Linq;

namespace Ryanair.Reservation.Validators.Implementations
{
    public class ReservationValidator : IReservationValidator
    {
        private const int ROUNDTRIP_FLIGHTS_COUNT = 2;

        public IEnumerable<string> ValidateReservation(Infrastructure.Business.Domain.Reservation reservation)
        {
            var errorMessages = new List<string>();

            if (reservation == null)
            {
                errorMessages.Add("Please, provide reservation.");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(reservation.Email))
            {
                errorMessages.Add("Please, provide email address.");
            }

            if (string.IsNullOrWhiteSpace(reservation.CreditCard))
            {
                errorMessages.Add("Please, provide credit card.");
            }

            if (reservation.Flights == null || !reservation.Flights.Any())
            {
                errorMessages.Add("Please, provide flights to reserve.");
                return errorMessages;
            }

            if(reservation.Flights.Length == ROUNDTRIP_FLIGHTS_COUNT && reservation.Flights[0].Key == reservation.Flights[1].Key)
            {
                errorMessages.Add("For roundtrip flights please, select different airplanes.");
            }

            return errorMessages;
        }
    }
}
