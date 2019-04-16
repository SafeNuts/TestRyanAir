using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Util;
using Ryanair.Reservation.Infrastructure.Business.Validators;

namespace Ryanair.Reservation.Infrastructure.Business.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPassengerService _passengerService;
        private readonly IMapper _mapper;
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerValidator _passengerValidator;
        private readonly IFlightService _flightService;

        public ReservationService(IReservationRepository reservationRepository, IMapper mapper, 
            IPassengerService passengerService, IFlightRepository flightRepository, 
            IPassengerValidator passengerValidator, IFlightService flightService)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _flightRepository = flightRepository;
            _passengerService = passengerService;
            _passengerValidator = passengerValidator;
            _flightService = flightService;
        }

        public string CreateReservation(Domain.Reservation reservation)
        {
            if(reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            var mappedReservation = _mapper.Map<ReservationEntity>(reservation);

            if (reservation.Flights == null || reservation.Flights.Length < 1)
            {
                throw new ArgumentException("Please, select flights to reserve.");
            }

            foreach (var flight in reservation.Flights)
            {
                _passengerValidator.ValidatePassengers(flight.Key, flight.Passengers);

                var flightFromDb = _flightRepository.Get(flight.Key);

                if(flightFromDb == null)
                {
                    throw new KeyNotFoundException($"Flight with key: {flight.Key} was not found.");
                }

                mappedReservation.Flights.Add(flightFromDb);
            }

            _flightService.CreatePassengersForFlights(reservation.Flights);

            mappedReservation.Key = GenerateReservationKey();

            var result = _reservationRepository.Create(mappedReservation);

            return result;
        }

        public Domain.Reservation GetReservationByKey(string reservationKey)
        {
            if (string.IsNullOrWhiteSpace(reservationKey))
            {
                throw new ArgumentNullException(nameof(reservationKey));
            }

            var reservation = _reservationRepository.Get(reservationKey);

            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with key: {reservationKey} was not found.");
            }

            var mappedReservation = _mapper.Map<Domain.Reservation>(reservation);

            foreach(var flight in mappedReservation.Flights)
            {
                var partners = _passengerService.GetPassengersByFlightKey(flight.Key);
                flight.Passengers = partners;
            }

            return mappedReservation;
        }

        private string GenerateReservationKey()
        {
            var key = KeyGenerator.GenerateKey();

            // Generate key till it will not be unique for database
            while (_reservationRepository.CheckIfKeyExists(key))
            {
                key = KeyGenerator.GenerateKey();
            }

            return key;
        }
    }
}
