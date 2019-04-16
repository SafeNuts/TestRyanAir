namespace Ryanair.Reservation.Infrastructure.Business.Services
{
    public interface IReservationService
    {
        Domain.Reservation GetReservationByKey(string reservationKey);

        string CreateReservation(Domain.Reservation reservation);
    }
}
