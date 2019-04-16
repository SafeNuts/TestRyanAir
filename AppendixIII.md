## Appendix III – GET /Reservation

This endpoint returns an existing reservation using the reservation number as parameter.

## Request contract url

/Reservation/ABC123

## Response contract in JSON format

```json
{
  "reservationNumber": "ABC123",
  "email": "contact@contact.com",
  "flights": [
    {
      "key": "Flight00052",
      "passengers": [
        {
          "name": "Robert Plant",
          "bags": 3,
          "seat": "27"
        },
        {
          "name": "Ozzy Osbourne",
          "bags": 0,
          "seat": "28"
        }
      ]
    },
    {
      "key": "Flight00103",
      "passengers": [
        {
          "name": "Robert Plant",
          "bags": 2,
          "seat": "41"
        },
        {
          "name": "Ozzy Osbourne",
          "seat": "40"
        }
      ]
    }
  ]
}
```

The reservation number should be an unique sequence of 3 letters and 3 numbers.
