using System;

namespace Ryanair.Reservation.Infrastructure.Business.Exceptions
{
    public class PassengerIsNotCreatedException : Exception
    {
        public PassengerIsNotCreatedException(string message) : base(message)
        { }
    }
}
