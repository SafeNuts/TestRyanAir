using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Models;
using Ryanair.Reservation.Validators;

namespace Ryanair.Reservation.Controllers
{
    [Route("api/flight")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IFlightSearchRequestValidator _flightSearchRequestValidator;

        public FlightController(IFlightService flightService, ILogger<FlightController> logger, IMapper mapper, IFlightSearchRequestValidator flightSearchRequestValidator)
        {
            _flightSearchRequestValidator = flightSearchRequestValidator;
            _flightService = flightService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int passengers, string origin, string destination, DateTime dateOut, DateTime? dateIn, bool roundTrip)
        {

            var validationErrorMessages = _flightSearchRequestValidator.ValidateFlightSearchRequest(passengers, origin, destination, dateOut, dateIn, roundTrip);

            if(validationErrorMessages.Any())
            {
                return new BadRequestObjectResult(validationErrorMessages);
            }

            try
            {
                var availableFlights = _flightService.SearchAvailableFlights(passengers, origin, destination, dateOut, dateIn, roundTrip);

                var mappedModels = _mapper.Map<IEnumerable<FlightViewModel>>(availableFlights);
                return new OkObjectResult(mappedModels);
            }
            catch(ArgumentException exception)
            {
                _logger.LogError($"Error has occured. Exception: {exception}");
                return new BadRequestObjectResult(exception.Message);
            }
            catch(Exception exception)
            {
                _logger.LogError($"Error has occured. Exception: {exception}");
                return StatusCode(500, exception.Message);
            }
        }
    }
}
