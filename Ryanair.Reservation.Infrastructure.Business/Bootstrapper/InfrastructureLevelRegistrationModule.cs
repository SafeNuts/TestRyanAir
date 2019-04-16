using Autofac;
using Ryanair.Reservation.Data.Interfaces;
using Ryanair.Reservation.Infrastructure.Business.Services;
using Ryanair.Reservation.Infrastructure.Business.Services.Implementations;
using Ryanair.Reservation.Infrastructure.Business.Validators;
using Ryanair.Reservation.Infrastructure.Business.Validators.Implementations;
using Ryanair.Reservation.Infrastructure.Data.Repositories;

namespace Ryanair.Reservation.Infrastructure.Business.Bootstrapper
{
    public class InfrastructureLevelRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRepositories(builder);
            RegisterServices(builder);
            RegisterValidators(builder);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<FlightRepository>().As<IFlightRepository>().SingleInstance();
            builder.RegisterType<ReservationRepository>().As<IReservationRepository>().SingleInstance();
            builder.RegisterType<PassengerRepository>().As<IPassengerRepository>().SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<FlightService>().As<IFlightService>();
            builder.RegisterType<ReservationService>().As<IReservationService>();
            builder.RegisterType<PassengerService>().As<IPassengerService>();
        }

        private void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<PassengerValidator>().As<IPassengerValidator>();
        }
    }
}
