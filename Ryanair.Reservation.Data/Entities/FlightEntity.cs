using System;

namespace Ryanair.Reservation.Data.Entities
{
    public class FlightEntity : BaseEntity
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Time { get; set; }
    }
}