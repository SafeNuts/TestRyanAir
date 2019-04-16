using AutoMapper;
using Moq;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Infrastructure.Business.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Business.Services
{
    public class FlightServiceTests
    {
        
        [Fact]
        public void ShouldReturnFlightsBySearchParameters()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var expectedResultFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                }
            };

            var availableFlights = new List<FlightEntity>()
            {
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                }
            };

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string VALID_DESTINATION = "LONDON";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Flight>>(It.Is<IEnumerable<FlightEntity>>(c => c.Count() == 1))).Returns(expectedResultFlights);

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, VALID_DESTINATION, flightOutDate, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedResultFlights));

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersNoFoundDestination()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string NON_EXISTING_DESTINATION = "NON_EXISTING_DESTINATION";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, NON_EXISTING_DESTINATION, flightOutDate, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersNoFoundOrigin()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string NON_EXISTING_ORIGIN = "NON_EXISTING_ORIGIN";
            const string DESTINATION = "LONDONG";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, NON_EXISTING_ORIGIN, DESTINATION, flightOutDate, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersNoFoundOutDate()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string DESTINATION = "LONDONG";
            var nonExistingFlightOutDate = new DateTime(2020, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, DESTINATION, nonExistingFlightOutDate, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersNoFlightsWithEnoughFreeSeats()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var expectedResultFlights = new List<Flight>();

            var availableFlights = new List<FlightEntity>();

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger()

            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string VALID_DESTINATION = "LONDON";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Flight>>(It.IsAny<IEnumerable<FlightEntity>>())).Returns(expectedResultFlights);

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, VALID_DESTINATION, flightOutDate, null, false);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldReturnOlyOutFlightsBySearchParametersNoFoundFlightsForInDate()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var expectedResultFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                }
            };

            var availableFlights = new List<FlightEntity>()
            {
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                }
            };

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string VALID_DESTINATION = "LONDON";
            var flightOutDate = new DateTime(2019, 04, 16);
            var nonExistingFlightInDate = new DateTime(2020, 04, 20);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Flight>>(It.Is<IEnumerable<FlightEntity>>(c => c.Count() == 1))).Returns(expectedResultFlights);


            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, VALID_DESTINATION, flightOutDate, nonExistingFlightInDate, true);

            // Assert
            Assert.NotNull(result);

            var listResult = result.ToList();
            Assert.True(listResult.SequenceEqual(expectedResultFlights));

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldReturnRoundTripFlightsBySearchParameters()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var expectedResultFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "DUBLIN",
                    Origin = "LONDON",
                    Key = "Flight0007",
                    Time = new DateTime(2019, 04, 20)
                }
            };

            var availableFlights = new List<FlightEntity>()
            {
                new FlightEntity()
                {
                    Destination = "DUBLIN",
                    Origin = "LONDON",
                    Key = "Flight0007",
                    Time = new DateTime(2019, 04, 20)
                }
            };

            var passengers = GeneratePassengersFullyBookedFlight();

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string VALID_DESTINATION = "LONDON";
            var flightOutDate = new DateTime(2019, 04, 16);
            var flightInDate = new DateTime(2019, 04, 20);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.Is<string>(c => c != "Flight0007"))).Returns(passengers);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Flight>>(It.Is<IEnumerable<FlightEntity>>(c => c.Count() == 1))).Returns(expectedResultFlights);

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, VALID_DESTINATION, flightOutDate, flightInDate, true);

            // Assert
            Assert.NotNull(result);

            var listResult = result.ToList();
            Assert.True(listResult.SequenceEqual(expectedResultFlights));

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldReturnOnlyReturnFlightsBySearchParametersNoToDestinationFlightsWithEmptySeats()
        {
            // Arrange
            var allFlights = CreateValidFlights();

            var expectedResultFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                },
                new Flight()
                {
                    Destination = "DUBLIN",
                    Origin = "LONDON",
                    Key = "Flight0007",
                    Time = new DateTime(2019, 04, 20)
                }
            };

            var availableFlights = new List<FlightEntity>()
            {
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                },
                new FlightEntity()
                {
                    Destination = "DUBLIN",
                    Origin = "LONDON",
                    Key = "Flight0007",
                    Time = new DateTime(2019, 04, 20)
                }
            };

            var passengers = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            const int PASSENGERS_NUMBER = 10;
            const string VALID_ORIGIN = "DUBLIN";
            const string VALID_DESTINATION = "LONDON";
            var flightOutDate = new DateTime(2019, 04, 16);
            var flightInDate = new DateTime(2019, 04, 20);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            flightRepositoryMock.Setup(x => x.GetAll()).Returns(allFlights);

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(It.IsAny<string>())).Returns(passengers);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Flight>>(It.Is<IEnumerable<FlightEntity>>(c => c.Count() == 1))).Returns(expectedResultFlights);

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = service.SearchAvailableFlights(PASSENGERS_NUMBER, VALID_ORIGIN, VALID_DESTINATION, flightOutDate, flightInDate, true);

            // Assert
            Assert.NotNull(result);

            var listResult = result.ToList();
            Assert.True(listResult.SequenceEqual(expectedResultFlights));

            flightRepositoryMock.VerifyAll();
            passengerServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ShouldNotReturnFlightsBySearchParametersNoPartnersNumberProvided(int passengersNumber)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Please, provide number of passengers greater then 0.";
            const string ORIGIN = "DUBLIN";
            const string DESTINATION = "LONDONG";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = Assert.Throws<ArgumentException>(() => service.SearchAvailableFlights(passengersNumber, ORIGIN, DESTINATION, flightOutDate, null, false));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotReturnFlightsBySearchParametersNoDestinationProvided(string destination)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: destination";
            const int PASSENGERS_NUMBER = 10;
            const string ORIGIN = "LONDONG";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = Assert.Throws<ArgumentNullException>(() => service.SearchAvailableFlights(PASSENGERS_NUMBER, ORIGIN, destination, flightOutDate, null, false));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotReturnFlightsBySearchParametersNoOriginProvided(string origin)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: origin";
            const int PASSENGERS_NUMBER = 10;
            const string DESTINATION = "LONDONG";
            var flightOutDate = new DateTime(2019, 04, 16);

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = Assert.Throws<ArgumentNullException>(() => service.SearchAvailableFlights(PASSENGERS_NUMBER, origin, DESTINATION, flightOutDate, null, false));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersEmptyDateOutProvided()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Please, provide valid date out.";
            const int PASSENGERS_NUMBER = 10;
            const string DESTINATION = "LONDONG";
            const string ORIGIN = "DUBLIN";
            var emptyDateOut = new DateTime();

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = Assert.Throws<ArgumentException>(() => service.SearchAvailableFlights(PASSENGERS_NUMBER, ORIGIN, DESTINATION, emptyDateOut, null, false));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldNotReturnFlightsBySearchParametersInvalidDateInProvided()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Please, provide valid date in.";
            const int PASSENGERS_NUMBER = 10;
            const string DESTINATION = "LONDONG";
            const string ORIGIN = "DUBLIN";
            var dateOut = new DateTime(2010, 1, 1);
            var nonValidFlightInDate = new DateTime();

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act
            var result = Assert.Throws<ArgumentException>(() => 
                service.SearchAvailableFlights(PASSENGERS_NUMBER, ORIGIN, DESTINATION, dateOut, nonValidFlightInDate, true));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Fact]
        public void ShouldCreatePassengersForFlights()
        {
            // Arrange
            var flightsToCreate = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "TestDescription",
                    Passengers = new List<Passenger>()
                    {
                        new Passenger(),
                        new Passenger()
                    }
                },
                new Flight()
                {
                    Destination = "TestDescription2",
                    Passengers = new List<Passenger>()
                    {
                        new Passenger(),
                        new Passenger()
                    }
                }
            };

            var flightRepositoryMock = new Mock<IFlightRepository>();

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.CreatePassengers(It.IsAny<string>(), It.IsAny<IEnumerable<Passenger>>()));

            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act, Assert
            service.CreatePassengersForFlights(flightsToCreate);

            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldNoCreatePassengersForEmptyFlightsCollectionProvided()
        {
            // Arrange
            var flightsToCreate = new List<Flight>();

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act, Assert
            service.CreatePassengersForFlights(flightsToCreate);
        }

        [Fact]
        public void ShouldNoCreatePassengersForNoFlightsCollectionProvidedThrowsArgumentNullException()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: flights";

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var passengerServiceMock = new Mock<IPassengerService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var service = new FlightService(flightRepositoryMock.Object, mapperMock.Object, passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => service.CreatePassengersForFlights(null));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        private IEnumerable<Passenger> GeneratePassengersFullyBookedFlight()
        {
            return new List<Passenger>()
            {
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger(),
                new Passenger()
            };
        }

        private IEnumerable<FlightEntity> CreateValidFlights()
        {
            return new List<FlightEntity>()
            {
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0001",
                    Time = new DateTime(2019, 04, 15)
                },
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0002",
                    Time = new DateTime(2019, 04, 16)
                },
                new FlightEntity()
                {
                    Destination = "LONDON",
                    Origin = "DUBLIN",
                    Key = "Flight0003",
                    Time = new DateTime(2019, 04, 17)
                },
                new FlightEntity()
                {
                    Destination = "DUBLIN",
                    Origin = "LONDON",
                    Key = "Flight0007",
                    Time = new DateTime(2019, 04, 20)
                },
            };
        }
    }
}
