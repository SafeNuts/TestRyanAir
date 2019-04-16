## Ryanair Reservation Test - TravelLabs

## Intro
The airline **EtlBlue** is creating a brand new API to expose its reservation mechanism to other inhouse systems as well as potentially interested third parties. 

To allow this a successful candidate must implement an API with the following features:

## Task requirements

 1. The API should expose the following operations (see appendixes for further details):
    * [GET /Flight](AppendixI.md): used to search for available flights on a certain date between two different locations.
    * [POST /Reservation](AppendixII.md): used to create a reservation in the system
    * [GET /Reservation](AppendixIII.md): used to retrieve a reservation previously made.
 2. System constraints:
    * There is a maximum of 50 bags per flight in total for all the passengers.
    * Each passenger can have a maximum of 5 bags per flight.
    * There are 50 seats available per flight, numbered sequentially: “01”, “02”… “50”.
    * The API should be able to accept and return JSON and XML payloads.
 3. Every endpoint should return appropriate error messages when the operation cannot be achieved for some reason. 
 4. For storage, use in-memory collections to avoid external dependencies. 
 5. For the initial data state, use the [provided json](InitialState.json).
 6. Implement appropriate test cases.

## Bonus
1. Make use of some mapping framework.
2. Make use of logging.

## What we are looking for?
You're allowed to add any particular framework you want and keep in mind that we are looking for clean and maintainable code which follows good programming principles.

    - Clean Code
    - SOLID Principles
    - Coding in english
    - Follow the requirements

## Submission

For the correct development of the test, it is necessary to take into account the following points:

- Create an account at **GitLab** (it's free)
- Fork the repository and work on the solution `Ryanair.Reservation.sln`.
- We want to see the evolution of your code, so commits are welcome.
- Please note that the application will be executed on other machines, so don't keep local references.

The solution includes a README_Candidate.md file to be used for any other consideration or explanation that the candidate wants to highlight about the design/implementation process.

---

Thanks for your time, we look forward to hearing from you!

Ryanair Reservation Team