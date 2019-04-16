using System;

namespace Ryanair.Reservation.Models
{
    public class FlightViewModel
    {
        public string Key { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Time { get; set; }
    }
}
