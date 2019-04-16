using System;
using System.Collections.Generic;
using Moq;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Infrastructure.Business.Validators.Implementations;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Business.Validators
{
    public class PassengerValidatorTests
    {
        [Fact]
        public void ShouldValidatePassengerWithNoExceptions()
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "06",
                    Bags = 4
                },
                new Passenger()
                {
                    Seat = "08",
                    Bags = 3
                }
            };

            var passengersFromFlight = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "01",
                    Bags = 3
                },
                new Passenger()
                {
                    Seat = "02",
                    Bags = 3
                }
            };

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(FLIGHT_KEY)).Returns(passengersFromFlight);

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);
            
            // Act, Assert
            validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate);

            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldValidatePassengerReturnsArgumentExceptionPassengersHaveSameSeatNumbers()
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "One or more seats were selected more than one time. Seats: #06.";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "06",
                    Bags = 4
                },
                new Passenger()
                {
                    Seat = "06",
                    Bags = 3
                }
            };

            var passengersFromFlight = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "01",
                    Bags = 3
                },
                new Passenger()
                {
                    Seat = "02",
                    Bags = 3
                }
            };

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(FLIGHT_KEY)).Returns(passengersFromFlight);

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);

            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldValidatePassengerReturnsArgumentExceptionAlreadyBookedSeatsProvided()
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "One or more seats are already booked. Seats: #06, 08.";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "06",
                    Bags = 4
                },
                new Passenger()
                {
                    Seat = "08",
                    Bags = 3
                }
            };

            var passengersFromFlight = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "06",
                    Bags = 3
                },
                new Passenger()
                {
                    Seat = "08",
                    Bags = 3
                }
            };

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(FLIGHT_KEY)).Returns(passengersFromFlight);

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldValidatePassengerReturnsArgumentExceptionNotEnoughBagPlaces()
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "Unfortunately, there is no place for such number of bags: 4.";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "10",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "11",
                    Bags = 4
                }
            };

            var passengersFromFlight = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "01",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "02",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "03",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "04",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "05",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "06",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "07",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "08",
                    Bags = 5
                },
                new Passenger()
                {
                    Seat = "09",
                    Bags = 5
                }
            };

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(FLIGHT_KEY)).Returns(passengersFromFlight);

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);

            passengerServiceMock.VerifyAll();
        }

        [Theory]
        [InlineData("wrongFormat")]
        [InlineData("100")]
        public void ShouldValidatePassengerReturnsArgumentExceptionNotNonValidPassengerSeats(string wrongSeat)
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "Passengers can take seats only between '01' and '50'.";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = wrongSeat,
                    Bags = 5
                }
            };

            var passengersFromFlight = new List<Passenger>()
            {
                new Passenger(),
                new Passenger()
            };

            var passengerServiceMock = new Mock<IPassengerService>();
            passengerServiceMock.Setup(x => x.GetPassengersByFlightKey(FLIGHT_KEY)).Returns(passengersFromFlight);

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);

            passengerServiceMock.VerifyAll();
        }

        [Fact]
        public void ShouldValidatePassengerWithArgumentNullException()
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: passengers";

            var passengerServiceMock = new Mock<IPassengerService>();

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => validator.ValidatePassengers(FLIGHT_KEY, null));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(6)]
        public void ShouldValidatePassengerReturnsArgumentExceptionInvalidBagsNumber(short bagsNumber)
        {
            // Arrange
            const string FLIGHT_KEY = "testFlightKey";
            const string EXPECTED_ERROR_MESSAGE = "Passenger can take only 0 - 5 bags.";

            var passengersToValidate = new List<Passenger>()
            {
                new Passenger()
                {
                    Seat = "06",
                    Bags = bagsNumber
                },
                new Passenger()
                {
                    Seat = "08",
                    Bags = bagsNumber
                }
            };

            var passengerServiceMock = new Mock<IPassengerService>();

            // SUT
            var validator = new PassengerValidator(passengerServiceMock.Object);

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => validator.ValidatePassengers(FLIGHT_KEY, passengersToValidate));

            Assert.Equal(result.Message, EXPECTED_ERROR_MESSAGE);
        }
    }
}
