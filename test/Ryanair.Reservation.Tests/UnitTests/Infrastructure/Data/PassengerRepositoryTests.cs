using System;
using System.Collections.Generic;
using System.Linq;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Infrastructure.Data.Repositories;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Data
{
    public class PassengerRepositoryTests
    {
        [Fact]
        public void ShouldCreatePassenger()
        {
            // Arrange
            var repository = new PassengerRepository();

            var expectedPassenger = new PassengerEntity()
            {
                Key = "testKey1"
            };

            // Act, Assert
            repository.Create(expectedPassenger);
        }

        [Fact]
        public void ShouldNotCreatePassengerNoModelProvided()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: passengerEntity";

            var repository = new PassengerRepository();

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => repository.Create(null));
            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Fact]
        public void ShouldGetAllPassengers()
        {
            // Arrange
            var repository = new PassengerRepository();

            var expectedPassenger = new PassengerEntity()
            {
                Key = "testKey1"
            };

            var additionalExpectedPassenger = new PassengerEntity()
            {
                Key = "testKey2"
            };

            var expectedResults = new List<PassengerEntity>()
            {
                expectedPassenger,
                additionalExpectedPassenger
            };

            repository.Create(expectedPassenger);
            repository.Create(additionalExpectedPassenger);

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();

            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedResults));
        }
    }
}
