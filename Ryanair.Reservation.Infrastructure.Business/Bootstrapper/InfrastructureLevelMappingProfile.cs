using AutoMapper;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Infrastructure.Business.Domain;

namespace Ryanair.Reservation.Infrastructure.Business.Bootstrapper
{
    public class InfrastructureLevelMappingProfile : Profile
    {
        public InfrastructureLevelMappingProfile()
        {
            CreateMap<Passenger, PassengerEntity>();
            CreateMap<Flight, FlightEntity>();
            CreateMap<Passenger, PassengerEntity>();

            CreateMap<Domain.Reservation, ReservationEntity>().ForMember(x => x.Flights, opt => opt.Ignore());
            CreateMap<FlightEntity, Flight>().ForMember(x => x.Passengers, opt => opt.Ignore());
        }
    }
}
