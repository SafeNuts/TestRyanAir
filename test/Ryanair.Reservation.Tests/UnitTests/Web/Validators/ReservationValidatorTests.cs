using System.Collections.Generic;
using System.Linq;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Validators.Implementations;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Web.Validators
{
    public class ReservationValidatorTests
    {
        [Fact]
        public void ShouldValidateReservationWithNoErrors()
        {
            // Arrange
            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Email = "testEmail",
                CreditCard = "testCreditCard",
                Flights = new[]
                {
                    new Flight()
                    {
                        Key = "testKey1"
                    },
                    new Flight()
                    {
                        Key = "testKey2"
                    } 
                }
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void ShouldValidateReservationReturnsErrorFlightsWithSameKeyProvided()
        {
            // Arrange
            var expectedErrorMessages = new List<string>()
            {
                "For roundtrip flights please, select different airplanes."
            };

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Email = "testEmail",
                CreditCard = "testCreditCard",
                Flights = new[]
                {
                    new Flight()
                    {
                        Key = "testKey1"
                    },
                    new Flight()
                    {
                        Key = "testKey1"
                    }
                }
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedErrorMessages));
        }

        [Fact]
        public void ShouldValidateReservationReturnsErrorEmptyFlightsProvided()
        {
            // Arrange
            var expectedErrorMessages = new List<string>()
            {
                "Please, provide flights to reserve."
            };

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Email = "testEmail",
                CreditCard = "testCreditCard",
                Flights = new Flight[0]
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedErrorMessages));
        }

        [Fact]
        public void ShouldValidateReservationReturnsErrorNoEmailProvided()
        {
            // Arrange
            var expectedErrorMessages = new List<string>()
            {
                "Please, provide email address."
            };

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Flights = new[]
                {
                    new Flight()
                    {
                        Key = "testKey1"
                    },
                    new Flight()
                    {
                        Key = "testKey2"
                    }
                }
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedErrorMessages));
        }

        [Fact]
        public void ShouldValidateReservationReturnsErrorNoCreditCardProvided()
        {
            // Arrange
            var expectedErrorMessages = new List<string>()
            {
                "Please, provide credit card."
            };

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                Email = "testEmail",
                Flights = new[]
                {
                    new Flight()
                    {
                        Key = "testKey1"
                    },
                    new Flight()
                    {
                        Key = "testKey2"
                    }
                }
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedErrorMessages));
        }

        [Fact]
        public void ShouldValidateReservationReturnsErrorNoFlightsProvided()
        {
            // Arrange
            var expectedErrorMessages = new List<string>()
            {
                "Please, provide flights to reserve."
            };

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail"
            };

            // SUT
            var validator = new ReservationValidator();

            // Act
            var result = validator.ValidateReservation(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedErrorMessages));
        }
    }
}
