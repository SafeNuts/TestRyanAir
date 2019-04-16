## Appendix II – POST /Reservation

This endpoint allows to make a reservation into the system, providing among other parameters the keys received from the previous calls.

`Note: The flight keys from GET and POST should match.`

## Request contract in JSON format

```json
{
  "email": "contact@contact.com",
  "creditCard": "0123456789012345",
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

This request contains the email and credit card to make the reservation and also a list of flights that can contain one (for one way flights) or two (for roundtrip flights) elements. 

Every flight contains the flight key obtained with the GET /Flight call and a list of passengers. For every passenger we need its name, number of bags and selected seat.

## Response contract in JSON format

```json
{
 "reservationNumber": "ABC123"
}
```
 
The response just contains the reservation number assigned during the booking process. 

The reservation number should be an unique sequence of 3 letters and 3 numbers.


