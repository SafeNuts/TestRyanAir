using System;
using System.Collections.Generic;
using System.Linq;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Infrastructure.Data.Repositories;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Infrastructure.Data
{
    public class ReservationRepositoryTests
    {
        [Fact]
        public void ShouldCreateReservation()
        {
            // Arrange
            const string EXPECTED_RESERVATION_KEY = "AAA000";

            var repository = new ReservationRepository();

            var reservationToCreate = new ReservationEntity()
            {
                Key = EXPECTED_RESERVATION_KEY
            };

            // Act
            var result = repository.Create(reservationToCreate);

            // Assert
            Assert.Equal(EXPECTED_RESERVATION_KEY, result);
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsArgumentNullException()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: reservationItem";

            var repository = new ReservationRepository();

            // Act, Assert
            var result = Assert.Throws<ArgumentNullException>(() => repository.Create(null));

            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsArgumentException()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Reservation should has a key.";

            var reservationToCreate = new ReservationEntity();
            var repository = new ReservationRepository();

            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => repository.Create(reservationToCreate));

            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Fact]
        public void ShouldGetAllReservations()
        {
            // Arrange
            const string EXPECTED_RESERVATION_KEY = "AAA000";
            const string ADDITIONAL_EXPECTED_RESERVATION_KEY = "AAA001";

            var repository = new ReservationRepository();

            var reservationToCreate = new ReservationEntity()
            {
                Key = EXPECTED_RESERVATION_KEY
            };

            var additionalReservationToCreate = new ReservationEntity()
            {
                Key = ADDITIONAL_EXPECTED_RESERVATION_KEY
            };

            var expectedResultList = new List<ReservationEntity>()
            {
                reservationToCreate,
                additionalReservationToCreate
            };

            repository.Create(reservationToCreate);
            repository.Create(additionalReservationToCreate);

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expectedResultList));
        }

        [Fact]
        public void ShouldGetReservationByKey()
        {
            // Arrange
            const string RESERVATION_KEY = "AAA000";

            var repository = new ReservationRepository();

            var reservationToCreate = new ReservationEntity()
            {
                Key = RESERVATION_KEY
            };

            repository.Create(reservationToCreate);

            // Act
            var result = repository.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservationToCreate, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotGetReservationByKeyThrowsArgumentNullExceptionNoKeyProvided(string invalidKey)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: key";

            var repository = new ReservationRepository();


            // Act
            var result = Assert.Throws<ArgumentNullException>(() => repository.Get(invalidKey));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotCheckIfKeyExistsNoKeyProvided(string invalidKey)
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Value cannot be null.\r\nParameter name: key";

            var repository = new ReservationRepository();


            // Act
            var result = Assert.Throws<ArgumentNullException>(() => repository.CheckIfKeyExists(invalidKey));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Fact]
        public void ShouldCheckIfReservationKeyExistsReturnsTrue()
        {
            // Arrange
            const string RESERVATION_KEY = "AAA000";

            var repository = new ReservationRepository();

            var reservationToCreate = new ReservationEntity()
            {
                Key = RESERVATION_KEY
            };

            repository.Create(reservationToCreate);

            // Act
            var result = repository.CheckIfKeyExists(RESERVATION_KEY);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldCheckIfReservationKeyExistsReturnsFalse()
        {
            // Arrange
            const string RESERVATION_KEY = "AAA000";

            var repository = new ReservationRepository();

            // Act
            var result = repository.CheckIfKeyExists(RESERVATION_KEY);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldNotGetReservationByKeyThrowsNotFoundExceptionWrongKeyProvided()
        {
            // Arrange
            const string RESERVATION_KEY = "AAA000";
            const string EXPECTED_ERROR_MESSAGE = "Reservation with key: AAA000 was not found.";

            var repository = new ReservationRepository();


            // Act
            var result = Assert.Throws<KeyNotFoundException>(() => repository.Get(RESERVATION_KEY));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }

        [Fact]
        public void ShouldNotCreateReservationsThrowsArgumentExceptionDuplicateKeys()
        {
            // Arrange
            const string EXPECTED_RESERVATION_KEY = "AAA000";
            const string EXPECTED_ERROR_MESSAGE = "An item with the same key has already been added. Key: AAA000";

            var repository = new ReservationRepository();

            var reservationToCreate = new ReservationEntity()
            {
                Key = EXPECTED_RESERVATION_KEY
            };

            var additionalReservationToCreate = new ReservationEntity()
            {
                Key = EXPECTED_RESERVATION_KEY
            };

            repository.Create(reservationToCreate);


            // Act, Assert
            var result = Assert.Throws<ArgumentException>(() => repository.Create(additionalReservationToCreate));
            Assert.Equal(EXPECTED_ERROR_MESSAGE, result.Message);
        }
    }
}
