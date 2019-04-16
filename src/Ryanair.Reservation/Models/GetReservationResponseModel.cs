using System.Collections.Generic;

namespace Ryanair.Reservation.Models
{
    public class GetReservationResponseModel
    {
        public string ReservationNumber { get; set; }
        public string Email { get; set; }

        public IEnumerable<GetReservationFlightResponseModel> Flights { get; set; }
    }
}
