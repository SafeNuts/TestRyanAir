namespace Ryanair.Reservation.Data.Entities
{
    public class PassengerEntity : BaseEntity
    {
        public string FlightId { get; set; }
        public string Name { get; set; }
        public short Bags { get; set; }
        public string Seat { get; set; }
    }
}
