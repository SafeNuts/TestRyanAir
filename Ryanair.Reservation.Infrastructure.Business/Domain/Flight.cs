using System;
using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Business.Domain
{
    public class Flight
    {
        public string Key { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Time { get; set; }

        public IEnumerable<Passenger> Passengers { get; set; }
    }
}
