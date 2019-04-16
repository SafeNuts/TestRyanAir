using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ryanair.Reservation.Controllers;
using Ryanair.Reservation.Infrastructure.Business.Domain;
using Ryanair.Reservation.Infrastructure.Business.Exceptions;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Models;
using Ryanair.Reservation.Validators;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.Web.Controllers
{
    public class ReservationControllerTests
    {
        [Fact]
        public void ShouldGetReservationByKey()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = "stringKey",
                Flights = new []
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            var mappedReservation = new GetReservationResponseModel()
            {
                Email = "testEmail",
                Flights = new []
                {
                    new GetReservationFlightResponseModel()
                    {
                        Key = "testKey",
                        Passengers = new List<GetReservationPassengerResponseModel>()
                        {
                            new GetReservationPassengerResponseModel()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.GetReservationByKey(RESERVATION_KEY)).Returns(reservation);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<GetReservationResponseModel>(reservation)).Returns(mappedReservation);

            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as OkObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultResponse = mappedResult.Value as GetReservationResponseModel;
            Assert.NotNull(mappedResultResponse);
            Assert.StrictEqual(mappedReservation, mappedResultResponse);

            reservationServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldNotGetReservationByKeyNoKeyProvided(string inValidReservationKey)
        {
            // Arrange
            var reservationServiceMock = new Mock<IReservationService>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            const string EXPECTED_ERROR_MESSAGE = "Please, provide reservation id.";

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(inValidReservationKey);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultResponse = mappedResult.Value as string;
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultResponse);
        }

        [Fact]
        public void ShouldNotGetReservationByKeyThrowsKeyNotFoundException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";
            const string EXPECTED_ERROR_MESSAGE = "testMessage";

            var expectedError = new KeyNotFoundException(EXPECTED_ERROR_MESSAGE);

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.GetReservationByKey(RESERVATION_KEY))
                .Throws(expectedError);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as NotFoundObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultResponse = mappedResult.Value as string;
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultResponse);

            reservationServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetReservationByKeyThrowsArgumentException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";
            const string EXPECTED_ERROR_MESSAGE = "testMessage";

            var expectedError = new ArgumentException(EXPECTED_ERROR_MESSAGE);

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.GetReservationByKey(RESERVATION_KEY))
                .Throws(expectedError);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultResponse = mappedResult.Value as string;
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultResponse);

            reservationServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetReservationByKeyGetReservationByKeyThrowsArgumentException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";
            const string EXPECTED_ERROR_MESSAGE = "testMessage";

            var expectedError = new ArgumentException(EXPECTED_ERROR_MESSAGE);

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.GetReservationByKey(RESERVATION_KEY))
                .Throws(expectedError);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultResponse = mappedResult.Value as string;
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultResponse);

            reservationServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotGetReservationByKeyThrowsAnyException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";
            const string EXPECTED_ERROR_MESSAGE = "testMessage";

            var expectedError = new Exception(EXPECTED_ERROR_MESSAGE);

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.GetReservationByKey(RESERVATION_KEY))
                .Throws(expectedError);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Get(RESERVATION_KEY);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as ObjectResult;
            Assert.NotNull(mappedResult);
            Assert.Equal(500, mappedResult.StatusCode);

            var mappedResultResponse = mappedResult.Value as string;
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultResponse);

            reservationServiceMock.VerifyAll();
            mapperMock.VerifyAll();
        }

        [Fact]
        public void ShouldCreateReservation()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            var expectedResult = new CreateReservationResultModel()
            {
                ReservationNumber = RESERVATION_KEY
            };

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.CreateReservation(reservation)).Returns(RESERVATION_KEY);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CreateReservationResultModel>(RESERVATION_KEY)).Returns(expectedResult);

            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(new List<string>());

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as OkObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultValue = mappedResult.Value as CreateReservationResultModel;
            Assert.NotNull(mappedResultValue);

            Assert.StrictEqual(expectedResult, mappedResultValue);

            reservationServiceMock.VerifyAll();
            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationNoReservationProvided()
        {
            // Arrange
            const string EXPECTED_ERROR_MESSAGE = "Please, provide reservation.";

            var reservationServiceMock = new Mock<IReservationService>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();
            var reservationValidatorMock = new Mock<IReservationValidator>();

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(null);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);
            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResult.Value);

            reservationServiceMock.VerifyAll();
            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationValidationFailed()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            var validationErrorMessages = new List<string>()
            {
                "testError"
            };

            var reservationServiceMock = new Mock<IReservationService>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(validationErrorMessages);

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultBody = mappedResult.Value as IEnumerable<string>;
            Assert.NotNull(mappedResultBody);

            Assert.True(mappedResultBody.SequenceEqual(validationErrorMessages));

            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsKeyNotFoundException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            const string EXPECTED_ERROR_MESSAGE = "testErrorMessage";
            var expectedError = new KeyNotFoundException(EXPECTED_ERROR_MESSAGE);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.CreateReservation(reservation)).Throws(expectedError);

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(new List<string>());

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as NotFoundObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultBody = mappedResult.Value as string;
            Assert.NotNull(mappedResultBody);

            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultBody);

            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsArgumentException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            const string EXPECTED_ERROR_MESSAGE = "testErrorMessage";
            var expectedError = new ArgumentException(EXPECTED_ERROR_MESSAGE);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.CreateReservation(reservation)).Throws(expectedError);

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(new List<string>());

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as BadRequestObjectResult;
            Assert.NotNull(mappedResult);

            var mappedResultBody = mappedResult.Value as string;
            Assert.NotNull(mappedResultBody);

            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultBody);

            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsPassengerIsNotCreatedException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            const string EXPECTED_ERROR_MESSAGE = "testErrorMessage";
            var expectedError = new PassengerIsNotCreatedException(EXPECTED_ERROR_MESSAGE);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.CreateReservation(reservation)).Throws(expectedError);

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(new List<string>());

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as ObjectResult;
            Assert.NotNull(mappedResult);
            Assert.Equal(500, mappedResult.StatusCode);

            var mappedResultBody = mappedResult.Value as string;
            Assert.NotNull(mappedResultBody);

            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultBody);

            reservationValidatorMock.VerifyAll();
        }

        [Fact]
        public void ShouldNotCreateReservationThrowsAnyException()
        {
            // Arrange
            const string RESERVATION_KEY = "ABC001";

            var reservation = new Reservation.Infrastructure.Business.Domain.Reservation()
            {
                CreditCard = "testCreditCard",
                Email = "testEmail",
                Key = RESERVATION_KEY,
                Flights = new[]
                {
                    new Flight()
                    {
                        Destination = "testDestination",
                        Key = "testKey",
                        Origin = "testOrigin",
                        Time = DateTime.Now,
                        Passengers = new List<Passenger>()
                        {
                            new Passenger()
                            {
                                Bags = 4,
                                Name = "testName",
                                Seat = "01"
                            }
                        }
                    }
                }
            };

            const string EXPECTED_ERROR_MESSAGE = "testErrorMessage";
            var expectedError = new Exception(EXPECTED_ERROR_MESSAGE);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ReservationController>>();

            var reservationServiceMock = new Mock<IReservationService>();
            reservationServiceMock.Setup(x => x.CreateReservation(reservation)).Throws(expectedError);

            var reservationValidatorMock = new Mock<IReservationValidator>();
            reservationValidatorMock.Setup(x => x.ValidateReservation(reservation)).Returns(new List<string>());

            // SUT
            var controller = new ReservationController(loggerMock.Object, reservationServiceMock.Object,
                mapperMock.Object, reservationValidatorMock.Object);

            // Act
            var result = controller.Post(reservation);

            // Assert
            Assert.NotNull(result);

            var mappedResult = result as ObjectResult;
            Assert.NotNull(mappedResult);
            Assert.Equal(500, mappedResult.StatusCode);

            var mappedResultBody = mappedResult.Value as string;
            Assert.NotNull(mappedResultBody);

            Assert.Equal(EXPECTED_ERROR_MESSAGE, mappedResultBody);

            reservationValidatorMock.VerifyAll();
        }
    }
}
