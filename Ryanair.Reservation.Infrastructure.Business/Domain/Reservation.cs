using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Business.Domain
{
    public class Reservation
    {
        public string Key { get; set; }
        public string Email { get; set; }
        public string CreditCard { get; set; }

        public Flight[] Flights { get; set; }
    }
}
