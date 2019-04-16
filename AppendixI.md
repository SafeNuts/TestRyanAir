## Appendix I – GET /Flight

This endpoint will be used to search for available flights.

## Request contract url

>/Flight?passengers=3&origin=DUBLIN&destination=LONDON&dateOut=2017-05-08&dateIn=2017-05-10&roundTrip=true

The meaning of the parameters is as follows:

- Passengers: number of passengers to include in the reservation.
- Origin: origin of the flight.
- Destination: destination of the flight.
- DateOut: date of the outbound flight.
- DateIn: date of the inbound flight (optional for one way flight searches).
- RoundTrip: boolean indicating if we want one way or round trip results.
- Don't change the url parameter names (it will be used by internal integration tests)

## Response contract in JSON format

```json
[
  {
    "time": "2017-05-08T06:30:00.000Z",
    "key": "Flight00001",
    "origin": "DUBLIN",
    "destination": "LONDON"
  },
  {
    "time": "2017-05-08T12:00:00.000Z",
    "key": "Flight00052",
    "origin": "DUBLIN",
    "destination": "LONDON"
  },
  {
    "time": "2017-05-10T09:30:00.000Z",
    "key": "Flight00103",
    "origin": "LONDON",
    "destination": "DUBLIN"
  }
]
```

The [full json file](InitialState.json) is included in the solution.

The result is an enumeration with the flights found. It could contain 0 or more result. Every flight will have the date and time, origin, destination and a key to identify it in the POST /Reservation call.



