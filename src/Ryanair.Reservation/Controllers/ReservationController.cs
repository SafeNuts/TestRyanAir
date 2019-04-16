using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ryanair.Reservation.Infrastructure.Business.Exceptions;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Models;
using Ryanair.Reservation.Validators;

namespace Ryanair.Reservation.Controllers
{
    [Route("api/reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IReservationValidator _reservationValidator;

        public ReservationController(
            ILogger<ReservationController> logger, 
            IReservationService reservationService, 
            IMapper mapper,
            IReservationValidator reservationValidator)
        {
            _reservationValidator = reservationValidator;
            _logger = logger;
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet("{reservationKey}")]
        public IActionResult Get(string reservationKey)
        {
            if(string.IsNullOrWhiteSpace(reservationKey))
            {
                return new BadRequestObjectResult("Please, provide reservation id.");
            }

            try
            {
                var reservation = _reservationService.GetReservationByKey(reservationKey);

                var mappedReservation = _mapper.Map<GetReservationResponseModel>(reservation);
                return new OkObjectResult(mappedReservation);
            }
            catch(KeyNotFoundException exception)
            {
                _logger.LogError($"Reservation was not found fot key: {reservationKey}");
                return new NotFoundObjectResult(exception.Message);
            }
            catch(ArgumentException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception occurred during search for reservation. Exception message: {exception.Message}");
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Infrastructure.Business.Domain.Reservation reservation)
        {
            if(reservation == null)
            {
                return new BadRequestObjectResult("Please, provide reservation.");
            }

            try
            {
                var validationErrorMessages = _reservationValidator.ValidateReservation(reservation);

                if (validationErrorMessages.Any())
                {
                    return new BadRequestObjectResult(validationErrorMessages);
                }

                var reservationKey = _reservationService.CreateReservation(reservation);
                return new OkObjectResult(reservationKey);
            }
            catch (KeyNotFoundException exception)
            {
                _logger.LogError($"Requested flight was not found. Exception message: {exception.Message}");
                return new NotFoundObjectResult(exception.Message);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError($"Exception occurred during creation of reservation. Wrong data provided. Exception message: {exception.Message}");
                return new BadRequestObjectResult(exception.Message);
            }
            catch (PassengerIsNotCreatedException exception)
            {
                _logger.LogError($"Exception occurred during creation of passenger. Exception message: {exception.Message}");
                return StatusCode(500, exception.Message);
            }
            catch(Exception exception)
            {
                _logger.LogError($"Exception occurred during creation of reservation. Exception message: {exception.Message}");
                return StatusCode(500, exception.Message);
            } 
        }
    }
}
