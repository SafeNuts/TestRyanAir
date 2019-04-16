using System.Collections.Generic;

namespace Ryanair.Reservation.Data.Entities
{
    public class ReservationEntity : BaseEntity
    {
        public string Email { get; set; }
        public string CreditCard { get; set; }

        public IList<FlightEntity> Flights { get; set; }

        public ReservationEntity()
        {
            Flights = new List<FlightEntity>();
        }
    }
}
