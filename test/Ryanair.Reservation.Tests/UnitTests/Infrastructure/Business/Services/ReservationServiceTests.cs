using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Infrastructure.Business.Services.Implementations;
using Ryanair.Reservation.Infrastructure.Business.Validators;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Business.Services
{
    public class ReservationServiceTests
    {
        [Fact]
        public void ShouldCreateReservation()
        {
            // Assert
            const string RESERVATION_CREDIT_CARD = "testCreditCardNumber";
            const string EXPECTED_CREATED_RESERVATION_KEY = "reservationKey";

            var passengersInFlights = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            var flightsToReserve = new []
            {
                new Flight()
                {
                    Key = "Flight001",
                    Passengers = passengersInFlights
                },
                new Flight()
                {
                    Key = "Flight002",
                    Passengers = passengersInFlights
                }
            };

            var reservationToCreate = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = RESERVATION_CREDIT_CARD,
                Flights = flightsToReserve
            };

            var flightByKey = new FlightEntity();

            var mappedReservationToCreate = new ReservationEntity()
            {
                CreditCard = RESERVATION_CREDIT_CARD
            };

            var passengerServiceMock = new Mock<IPassengerService>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ReservationEntity>(reservationToCreate)).Returns(mappedReservationToCreate);

            var passengerValidatorMock = new Mock<IPassengerValidator>();
            passengerValidatorMock.Setup(x => x.ValidatePassengers(It.IsAny<string>(), passengersInFlights));

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(flightByKey);

            var flightServiceMock = new Mock<IFlightService>();
            flightServiceMock.Setup(x => x.CreatePassengersForFlights(flightsToReserve));

            var reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationRepositoryMock.Setup(x => x.CheckIfKeyExists(It.IsAny<string>())).Returns(false);
            reservationRepositoryMock.Setup(x =>
                x.Create(It.Is<ReservationEntity>(c => c.CreditCard == RESERVATION_CREDIT_CARD))).Returns(EXPECTED_CREATED_RESERVATION_KEY);

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act
            var result = service.CreateReservation(reservationToCreate);

            // Arrange
            Assert.True(result == EXPECTED_CREATED_RESERVATION_KEY);

            mapperMock.VerifyAll();
            passengerValidatorMock.VerifyAll();
            flightRepositoryMock.VerifyAll();
            flightServiceMock.VerifyAll();
            reservationRepositoryMock.VerifyAll();
        }

        
        [Fact]
        public void ShouldNotCreateReservationFlightWasNotFound()
        {
            // Assert
            const string RESERVATION_CREDIT_CARD = "testCreditCardNumber";
            const string EXPECTED_CREATED_RESERVATION_KEY = "reservationKey";

            var passengersInFlights = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            var flightsToReserve = new[]
            {
                new Flight()
                {
                    Key = "Flight001",
                    Passengers = passengersInFlights
                },
                new Flight()
                {
                    Key = "Flight002",
                    Passengers = passengersInFlights
                }
            };

            var reservationToCreate = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = RESERVATION_CREDIT_CARD,
                Flights = flightsToReserve
            };

            var flightByKey = new FlightEntity();

            var mappedReservationToCreate = new ReservationEntity()
            {
                CreditCard = RESERVATION_CREDIT_CARD
            };

            var passengerServiceMock = new Mock<IPassengerService>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ReservationEntity>(reservationToCreate)).Returns(mappedReservationToCreate);

            var passengerValidatorMock = new Mock<IPassengerValidator>();
            passengerValidatorMock.Setup(x => x.ValidatePassengers(It.IsAny<string>(), passengersInFlights));

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);

            var flightServiceMock = new Mock<IFlightService>();
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act
            var result = Assert.Throws<KeyNotFoundException>(() => service.CreateReservation(reservationToCreate));

            // Arrange
            Assert.True(result.Message == "Flight with key: Flight001 was not found.");

            mapperMock.VerifyAll();
            passengerValidatorMock.VerifyAll();
            flightRepositoryMock.VerifyAll();
            flightServiceMock.VerifyAll();
            reservationRepositoryMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationNoReservationProvided()
        {
            // Arrange
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();
            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: reservation";

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => service.CreateReservation(null));

            Assert.True(result.Message == EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldNotCreateReservationReservationFlightsModelProvided()
        {
            // Arrange
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();
            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            var reservationWithNoFlights = new Reservation.Infrastructure.Business.Domain.Reservation();

            const string EXPECTED_ERROR_MESSAGE = "Please, select flights to reserve.";

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => service.CreateReservation(reservationWithNoFlights));

            Assert.True(result.Message == EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldNotCreateReservationReservationFlightsProvided()
        {
            // Arrange
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();
            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            var reservationWithNoFlights = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Flights = new Flight[0]
            };

            const string EXPECTED_ERROR_MESSAGE = "Please, select flights to reserve.";

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => service.CreateReservation(reservationWithNoFlights));

            Assert.True(result.Message == EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldGetReservationByKey()
        {
            // Arrange
            const string RESERVATION_KEY = "reservationKey";
            const int EXPECTED_PASSENGERS_COUNT = 2;

            var reservationEntity = new ReservationEntity();
            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Flights = new[]
                {
                    new Flight(),
                    new Flight()
                }
            };

            var passengers = new List<Passenger>()
            {
                new Passenger()
            };

            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();

            var reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationRepositoryMock.Setup(x => x.Get(RESERVATION_KEY)).Returns(reservationEntity);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<Reservation.Infrastructure.Business.Domain.Reservation>(reservationEntity))
                .Returns(reservation);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act
            var result = service.GetReservationByKey(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Flights.Length == reservation.Flights.Length);
            Assert.True(result.Flights.Select(x => x.Passengers).Count() == EXPECTED_PASSENGERS_COUNT);

            reservationRepositoryMock.VerifyAll();
            mapperMock.VerifyAll();
            passengerServiceMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotGetReservationByKeyThrowsArgumentNullExceptionNoReservationKeyProvided(string reservationKey)
        {
            // Arrange
            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();
            var reservationRepositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var passengerServiceMock = new Mock<IPassengerService>();

            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: reservationKey";

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => service.GetReservationByKey(reservationKey));

            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);

            reservationRepositoryMock.VerifyAll();
            mapperMock.VerifyAll();
            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetReservationByKeyThrowsKeyNotFoundExceptionNoReservationFoundWithProvidedKey()
        {
            // Arrange
            const string RESERVATION_KEY = "reservationKey";

            var passengerValidatorMock = new Mock<IPassengerValidator>();
            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flightServiceMock = new Mock<IFlightService>();
            var mapperMock = new Mock<IMapper>();
            var passengerServiceMock = new Mock<IPassengerService>();

            var reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationRepositoryMock.Setup(x => x.Get(RESERVATION_KEY)).Returns(() => null);

            const string EXPECTED_ERROR_MESSAGE = "Reservation with key: reservationKey was not found.";

            // SUT
            var service = new ReservationService(reservationRepositoryMock.Object, mapperMock.Object,
                passengerServiceMock.Object, flightRepositoryMock.Object, passengerValidatorMock.Object,
                flightServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<KeyNotFoundException>(() => service.GetReservationByKey(RESERVATION_KEY));

            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);

            reservationRepositoryMock.VerifyAll();
        }
    }
}
