using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ryanair.Reservation.Controllers;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Models;
using Ryanair.Reservation.Validators;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Web.Controllers
{
    public class FlightControllerTests
    {
        [Fact]
        public void ShouldGetFlightsForSearchParametersWithNoRoundTrip()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;

            var searchedFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "testDestination"
                }
            };

            var expectedFlightInfo = new List<FlightViewModel>()
            {
                new FlightViewModel()
                {
                    Destination = "testDestination"
                }
            };

            var loggerMock = new Mock<ILogger<FlightController>>();

            var flightSearchRequestValidator = new Mock<IFlightSearchRequestValidator>();
            flightSearchRequestValidator.Setup(x => x.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER,
                ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Returns(new List<string>());

            var flightServiceMock = new Mock<IFlightService>();
            flightServiceMock.Setup(x => x.SearchAvailableFlights(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Returns(searchedFlights);


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<FlightViewModel>>(searchedFlights)).Returns(expectedFlightInfo);

            // SUT
            var controller = new FlightController(flightServiceMock.Object, loggerMock.Object, mapperMock.Object, flightSearchRequestValidator.Object);

            // Act
            var result = controller.Get(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as OkObjectResult;
            Assert.NotNull(mappedResult);

            var resultObject = mappedResult.Value as IEnumerable<FlightViewModel>;
            Assert.NotNull(resultObject);
            Assert.True(resultObject.SequenceEqual(expectedFlightInfo));

            flightServiceMock.VerifyAll();
            mapperMock.VerifyAll();
            flightSearchRequestValidator.VerifyAll();
        }

        [Fact]
        public void ShouldGetFlightsForSearchParametersWithRoundTrip()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            var searchedFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "testDestination"
                }
            };

            var expectedFlightInfo = new List<FlightViewModel>()
            {
                new FlightViewModel()
                {
                    Destination = "testDestination"
                }
            };

            var flightSearchRequestValidator = new Mock<IFlightSearchRequestValidator>();
            flightSearchRequestValidator.Setup(x => x.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER,
                ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Returns(new List<string>());

            var loggerMock = new Mock<ILogger<FlightController>>();
            var flightServiceMock = new Mock<IFlightService>();
            flightServiceMock.Setup(x => x.SearchAvailableFlights(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true)).Returns(searchedFlights);


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<FlightViewModel>>(searchedFlights)).Returns(expectedFlightInfo);

            // SUT
            var controller = new FlightController(flightServiceMock.Object, loggerMock.Object, mapperMock.Object, flightSearchRequestValidator.Object);

            // Act
            var result = controller.Get(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as OkObjectResult;
            Assert.NotNull(mappedResult);

            var resultObject = mappedResult.Value as IEnumerable<FlightViewModel>;
            Assert.NotNull(resultObject);
            Assert.True(resultObject.SequenceEqual(expectedFlightInfo));

            flightServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetFlightsForSearchParametersValidationFailed()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;
            var dateIn = DateTime.Now;

            const string EXPECTED_ERROR_MESSAGE = "testErrorMessage";

            var expectedErrorMessages = new List<string>() {EXPECTED_ERROR_MESSAGE};

            var flightSearchRequestValidator = new Mock<IFlightSearchRequestValidator>();
            flightSearchRequestValidator.Setup(x => x.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER,
                ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true)).Returns(expectedErrorMessages);

            var loggerMock = new Mock<ILogger<FlightController>>();
            var flightServiceMock = new Mock<IFlightService>();
            var mapperMock = new Mock<IMapper>();

            // SUT
            var controller = new FlightController(flightServiceMock.Object, loggerMock.Object, mapperMock.Object, flightSearchRequestValidator.Object);

            // Act
            var result = controller.Get(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, dateIn, true);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var resultObject = mappedResult.Value as IEnumerable<string>;
            Assert.NotNull(resultObject);
            Assert.True(resultObject.SequenceEqual(expectedErrorMessages));

            flightServiceMock.VerifyAll();
            mapperMock.VerifyAll();
            flightSearchRequestValidator.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetFlightsForSearchParametersThrowsArgumentException()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;

            const string EXPECTED_ERROR_MESSAGE = "testError";

            var expectedErrorMessage = new ArgumentException(EXPECTED_ERROR_MESSAGE);

            var searchedFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "testDestination"
                }
            };

            var expectedFlightInfo = new List<FlightViewModel>()
            {
                new FlightViewModel()
                {
                    Destination = "testDestination"
                }
            };

            var loggerMock = new Mock<ILogger<FlightController>>();

            var flightSearchRequestValidator = new Mock<IFlightSearchRequestValidator>();
            flightSearchRequestValidator.Setup(x => x.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER,
                ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Returns(new List<string>());

            var flightServiceMock = new Mock<IFlightService>();
            flightServiceMock.Setup(x => x.SearchAvailableFlights(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Throws(expectedErrorMessage);


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<FlightViewModel>>(searchedFlights)).Returns(expectedFlightInfo);

            // SUT
            var controller = new FlightController(flightServiceMock.Object, loggerMock.Object, mapperMock.Object, flightSearchRequestValidator.Object);

            // Act
            var result = controller.Get(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var resultObject = mappedResult.Value as string;
            Assert.NotNull(resultObject);
            Assert.Equal(resultObject, EXPECTED_ERROR_MESSAGE);

            flightServiceMock.VerifyAll();
            flightSearchRequestValidator.VerifyAll();
            loggerMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetFlightsForSearchParametersThrowsAny()
        {
            // Arrange
            const int PARTNERS_SEARCH_PARAMETER = 4;
            const string ORIGIN_SEARCH_PARAMETER = "DUBLIN";
            const string DESTINATION_SEARCH_PARAMETER = "LONDON";
            var dateOut = DateTime.Now;

            const string EXPECTED_ERROR_MESSAGE = "testError";

            var expectedErrorMessage = new Exception(EXPECTED_ERROR_MESSAGE);

            var searchedFlights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = "testDestination"
                }
            };

            var expectedFlightInfo = new List<FlightViewModel>()
            {
                new FlightViewModel()
                {
                    Destination = "testDestination"
                }
            };

            var loggerMock = new Mock<ILogger<FlightController>>();

            var flightSearchRequestValidator = new Mock<IFlightSearchRequestValidator>();
            flightSearchRequestValidator.Setup(x => x.ValidateFlightSearchRequest(PARTNERS_SEARCH_PARAMETER,
                ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Returns(new List<string>());

            var flightServiceMock = new Mock<IFlightService>();
            flightServiceMock.Setup(x => x.SearchAvailableFlights(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false)).Throws(expectedErrorMessage);


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<FlightViewModel>>(searchedFlights)).Returns(expectedFlightInfo);

            // SUT
            var controller = new FlightController(flightServiceMock.Object, loggerMock.Object, mapperMock.Object, flightSearchRequestValidator.Object);

            // Act
            var result = controller.Get(PARTNERS_SEARCH_PARAMETER, ORIGIN_SEARCH_PARAMETER,
                DESTINATION_SEARCH_PARAMETER, dateOut, null, false);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as ObjectResult;
            Assert.NotNull(mappedResult);
            Assert.Equal(500, mappedResult.StatusCode);
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResult.Value);

            flightServiceMock.VerifyAll();
            flightSearchRequestValidator.VerifyAll();
            loggerMock.VerifyAll();
        }
    }
}
