using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;

namespace Ryanair.Reservation.Infrastructure.Business.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly IPassengerService _passengerService;

        private const short MAXIMUM_FLIGHT_PASSENGERS_NUMBER = 50;

        public FlightService(IFlightRepository flightRepository, IMapper mapper, IPassengerService passengerService)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
            _passengerService = passengerService;
        }

        public IEnumerable<Flight> SearchAvailableFlights(int passengers, string origin, string destination, DateTime dateOut, DateTime? dateIn, bool roundTrip)
        {
            var availableFlights = SearchAvailableFlights(passengers, origin, destination, dateOut);

            if (!roundTrip || dateIn == null)
            {
                return availableFlights;
            }

            if (dateIn == new DateTime())
            {
                throw new ArgumentException("Please, provide valid date in.");
            }

            var availableInFlights = SearchAvailableFlights(passengers, destination, origin, dateIn.Value);
            availableFlights = availableFlights.Union(availableInFlights);

            return availableFlights;
        }

        private IEnumerable<Flight> SearchAvailableFlights(int passengers, string origin, string destination, DateTime dateOut)
        {
            if (passengers <= 0)
            {
                throw new ArgumentException("Please, provide number of passengers greater then 0.");
            }

            if (string.IsNullOrWhiteSpace(origin))
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (dateOut == new DateTime())
            {
                throw new ArgumentException("Please, provide valid date out.");
            }

            var flights = _flightRepository.GetAll();

            var filterQuery = flights.Where(x => _passengerService.GetPassengersByFlightKey(x.Key).Count() + passengers <= MAXIMUM_FLIGHT_PASSENGERS_NUMBER
                                    && string.Equals(x.Origin, origin, StringComparison.CurrentCultureIgnoreCase)
                                    && string.Equals(x.Destination, destination, StringComparison.CurrentCultureIgnoreCase)
                                    && x.Time.ToShortDateString() == dateOut.ToShortDateString());

            var resultFlights = filterQuery.ToList();

            return _mapper.Map<IEnumerable<Flight>>(resultFlights);
        }

        public void CreatePassengersForFlights(IEnumerable<Flight> flights)
        {
            if (flights == null)
            {
                throw new ArgumentNullException(nameof(flights));
            }

            foreach (var flight in flights)
            {
                _passengerService.CreatePassengers(flight.Key, flight.Passengers);
            }
        }
    }
}
