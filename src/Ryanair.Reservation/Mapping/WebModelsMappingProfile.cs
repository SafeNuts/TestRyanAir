using AutoMapper;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Models;

namespace Ryanair.Reservation.Mapping
{
    public class WebModelsMappingProfile : Profile
    {
        public WebModelsMappingProfile()
        {
            CreateMap<Flight, FlightViewModel>();
            CreateMap<Infrastructure.Business.Domain.Reservation, GetReservationResponseModel>()
                .ForMember(x => x.ReservationNumber, opt => opt.MapFrom(x => x.Key));

            CreateMap<Passenger, GetReservationPassengerResponseModel>();
            CreateMap<Flight, GetReservationFlightResponseModel>();

            CreateMap<string, CreateReservationResultModel>()
                .ForMember(x => x.ReservationNumber, opt => opt.MapFrom(x => x));
        }
    }
}
