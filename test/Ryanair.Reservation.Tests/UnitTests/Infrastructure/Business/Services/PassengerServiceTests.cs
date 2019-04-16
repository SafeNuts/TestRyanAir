using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Exceptions;
using Ryanair.Reservation.Infrastructure.Business.Services.Implementations;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Business.Services
{
    public class PassengerServiceTests
    {
        [Fact]
        public void ShouldCreatePassengers()
        {
            // Arrange
            var passengerEntities = new List<PassengerEntity>()
            {
                new PassengerEntity(),
                new PassengerEntity()
            };

            var passengersToCreate = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const string CREATED_PASSENGER_KEY = "123";
            const string FLIGHT_KEY = "123";

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            passengerRepositoryMock.Setup(x => x.Create(It.IsAny<PassengerEntity>()))
                .Returns(CREATED_PASSENGER_KEY);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<PassengerEntity>>(passengersToCreate)).Returns(passengerEntities);

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act, Assert
            service.CreatePassengers(FLIGHT_KEY, passengersToCreate);

            passengerRepositoryMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotCreatePassengersThrowsPassengerIsNotCreatedException(string createdInDbKey)
        {
            // Arrange
            var passengerEntities = new List<PassengerEntity>()
            {
                new PassengerEntity(),
                new PassengerEntity()
            };

            var passengersToCreate = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const string FLIGHT_KEY = "123";

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            passengerRepositoryMock.Setup(x => x.Create(It.IsAny<PassengerEntity>()))
                .Returns(createdInDbKey);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<PassengerEntity>>(passengersToCreate)).Returns(passengerEntities);

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act, Assert
            Assert.Throws<PassengerIsNotCreatedException>(() => service.CreatePassengers(FLIGHT_KEY, passengersToCreate));

            passengerRepositoryMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotCreatePassengersThrowsArgumentNullExceptionNoFlightKeyProvided(string flightKey)
        {
            // Arrange
            var passengersToCreate = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: flightKey";

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => service.CreatePassengers(flightKey, passengersToCreate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldNotCreatePassengersThrowsArgumentNullExceptionNoPassengersProvided()
        {
            // Arrange
            const string FLIGHT_KEY = "123";
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: passengers";

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => service.CreatePassengers(FLIGHT_KEY, null));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldGetPassengersByProvidedFlightKey()
        {
            // Arrange
            const string FLIGHT_KEY = "Flight001";

            var passengerEntities = new List<PassengerEntity>()
            {
                new PassengerEntity()
                {
                    FlightId = FLIGHT_KEY,
                    Bags = 4,
                    Name = "TestPartner1",
                    Seat = "12"
                },
                new PassengerEntity()
                {
                    FlightId = FLIGHT_KEY,
                    Bags = 3,
                    Name = "TestPartner2",
                    Seat = "11"
                },
                new PassengerEntity()
                {
                    FlightId = "Flight002",
                    Bags = 3,
                    Name = "TestPartner3",
                    Seat = "11"
                }
            };

            var resultPassengers = new List<Passenger>()
            {
                new Passenger()
                {
                    Bags = 4,
                    Name = "TestPartner1",
                    Seat = "12"
                },
                new Passenger()
                {
                    Bags = 3,
                    Name = "TestPartner2",
                    Seat = "11"
                },
            };

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            passengerRepositoryMock.Setup(x => x.GetAll())
                .Returns(passengerEntities);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<Passenger>>(It.IsAny<IEnumerable<PassengerEntity>>())).Returns(resultPassengers);

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = service.GetPassengersByFlightKey(FLIGHT_KEY);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(resultPassengers));

            mapperMock.VerifyAll();
            passengerRepositoryMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetPassengersByProvidedFlightKeyNoMatchedFlightKey()
        {
            // Arrange
            const string FLIGHT_KEY = "Flight001";
            const string NON_EXISTING_FLIGHT_KEY = "Flight009";

            var passengerEntities = new List<PassengerEntity>()
            {
                new PassengerEntity()
                {
                    FlightId = FLIGHT_KEY,
                    Bags = 4,
                    Name = "TestPartner1",
                    Seat = "12"
                },
                new PassengerEntity()
                {
                    FlightId = FLIGHT_KEY,
                    Bags = 3,
                    Name = "TestPartner2",
                    Seat = "11"
                },
                new PassengerEntity()
                {
                    FlightId = "Flight002",
                    Bags = 3,
                    Name = "TestPartner3",
                    Seat = "11"
                }
            };

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            passengerRepositoryMock.Setup(x => x.GetAll())
                .Returns(passengerEntities);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<Passenger>>(It.IsAny<IEnumerable<PassengerEntity>>())).Returns(new List<Passenger>());

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = service.GetPassengersByFlightKey(NON_EXISTING_FLIGHT_KEY);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            mapperMock.VerifyAll();
            passengerRepositoryMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotGetPassengersByProvidedFlightKeyNoFlightKey(string flightKey)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: flightKey";

            var passengerRepositoryMock = new Mock<IPassengerRepository>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new PassengerService(passengerRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = Assert.Throws<ArgumentNullException>(() => service.GetPassengersByFlightKey(flightKey));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }
    }
}
