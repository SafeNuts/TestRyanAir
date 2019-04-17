## Ryanair Reservation Test - TravelLabs

## General Overview
Project represents an implementation of Ryanair Interview Task.

## Quick start

 To start project you will need:

- Visual Studio 2017 (or any other IDE which is able to work with .net core);
- .Net Core package installed on machine.

Start process steps:

- Open solution in Visual Studio;
- Set 'Ryanair.Reservation' as startup project;
- Launch application using F5.

Unit tests launch process:

- Open solution in Visual Studio;
- Build application using 'Ctrl+Shift+B' or using 'Build' tab;
- Open 'Test Explorer';
- Click on 'Run All' to run all tests.

## Endpoints list

 The API exposes following operations:
 - GET /Flight: used to search for available flights on a certain date between two different locations.
 - POST /Reservation: used to create a reservation in the system
 - GET /Reservation: used to retrieve a reservation previously made.

## Architecture overview

To implement defined functional requirements was decided to use "Onion" architecture.
It was chosen  to achieve clear separation of concepts and make layers loosely coupled.

Project structure:
- Ryanair.Reservation.Data - class library, contains entities and repository interfaces;
- Ryanair.Reservation.Infrastructure.Data - class library, containing implementations of repositories;
- Ryanair.Reservation.Infrastructure.Business - class library, containing business services and domain models;
- Ryanair.Reservation.Tests - test project, containing tests for different layers;
- Ryanair.Reservation - Asp.Net Core web-application, conatining endpoints.