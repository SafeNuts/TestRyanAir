using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ryanair.Reservation.Validators.Implementations;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Web.Validators
{
    public class FlightSearchRequestValidatorTests
    {
        [Fact]
        public void ShouldValidateFlightSearchRequestWithNoErrorsWithRoundTrip()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(51)]
        public void ShouldValidateFlightSearchRequestWithInvalidPassengerError(int invalidPassengers)
        {
            // Arrange
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            var expectedErrors = new List<string>() {"Please, provide number of passengers between 0 and 50."};

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(invalidPassengers, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldValidateFlightSearchRequestWithInvalidOriginError(string invalidOrigin)
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            var expectedErrors = new List<string>() { "Please, provide origin of the flight." };

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, invalidOrigin,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldValidateFlightSearchRequestWithInvalidDestinationError(string invalidDestination)
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            var expectedErrors = new List<string>() { "Please, provide destination of the flight." };

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                invalidDestination, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Fact]
        public void ShouldValidateFlightSearchRequestWithInvalidOutFlightError()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateIn = DateTime.Now;
            var invalidDateOut = new DateTime();

            var expectedErrors = new List<string>() { "Please, provide valid date out." };

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, invalidDateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Fact]
        public void ShouldValidateFlightSearchRequestWithInvalidDateInError()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var invalidDateIn = new DateTime();
            var dateOut = DateTime.Now;

            var expectedErrors = new List<string>() { "Please, provide valid date in." };

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, invalidDateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Fact]
        public void ShouldValidateFlightSearchRequestWithNoDateInError()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;

            var expectedErrors = new List<string>() { "Please, provide valid date in." };

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.True(resultList.Any());
            Assert.True(resultList.SequenceEqual(expectedErrors));
        }

        [Fact]
        public void ShouldValidateFlightSearchRequestWithNoErrorsNoRoundTripFlagProvided()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            // SUT
            var validator = new FlightSearchRequestValidator();

            // Act
            var result = validator.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var resultList = result.ToList();
            Assert.False(resultList.Any());
        }
    }
}
