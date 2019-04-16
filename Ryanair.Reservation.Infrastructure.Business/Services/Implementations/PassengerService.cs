using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Exceptions;

namespace Ryanair.Reservation.Infrastructure.Business.Services.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;

        public PassengerService(IPassengerRepository passengerRepository, IMapper mapper)
        {
            _passengerRepository = passengerRepository;
            _mapper = mapper;
        }

        public IEnumerable<Passenger> GetPassengersByFlightKey(string flightKey)
        {
            if (string.IsNullOrWhiteSpace(flightKey))
            {
                throw new ArgumentNullException(nameof(flightKey));
            }

            var allPassengers = _passengerRepository.GetAll();

            var filtered = allPassengers.Where(x => x.FlightId == flightKey);

            return _mapper.Map<IEnumerable<Passenger>>(filtered);
        }

        public void CreatePassengers(string flightKey, IEnumerable<Passenger> passengers)
        {
            if (string.IsNullOrWhiteSpace(flightKey))
            {
                throw new ArgumentNullException(nameof(flightKey));
            }

            if (passengers == null)
            {
                throw new ArgumentNullException(nameof(passengers));
            }

            var passengerEntities = _mapper.Map<IEnumerable<PassengerEntity>>(passengers);

            foreach(var passengerEntity in passengerEntities)
            {
                passengerEntity.FlightId = flightKey;

                // Since we don't use ORM to generate ids, we will set random number as a key
                passengerEntity.Key = DateTime.Now.Ticks.ToString();
                var createdPassengerKey = _passengerRepository.Create(passengerEntity);

                if (string.IsNullOrWhiteSpace(createdPassengerKey))
                {
                    throw new PassengerIsNotCreatedException($"Error occured during creation of passenger with key: {passengerEntity.Key}.");
                }
            }
        }
    }
}
