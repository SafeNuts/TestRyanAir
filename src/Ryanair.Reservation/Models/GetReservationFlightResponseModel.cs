using System.Collections.Generic;

namespace Ryanair.Reservation.Models
{
    public class GetReservationFlightResponseModel
    {
        public string Key { get; set; }

        public IEnumerable<GetReservationPassengerResponseModel> Passengers { get; set; }
    }
}
