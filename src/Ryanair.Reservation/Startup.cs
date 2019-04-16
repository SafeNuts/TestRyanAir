using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ryanair.Reservation.Infrastructure.Business.Bootstrapper;
using Ryanair.Reservation.Infrastructure.Business.Validators;
using Ryanair.Reservation.Infrastructure.Business.Validators.Implementations;
using Ryanair.Reservation.Mapping;
using Ryanair.Reservation.Validators;
using Ryanair.Reservation.Validators.Implementations;

namespace Ryanair.Reservation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WebModelsMappingProfile());
                mc.AddProfile(new InfrastructureLevelMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc()
                .AddControllersAsServices()
                .AddXmlSerializerFormatters();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new InfrastructureLevelRegistrationModule());

            builder.RegisterType<ReservationValidator>().As<IReservationValidator>();
            builder.RegisterType<FlightSearchRequestValidator>().As<IFlightSearchRequestValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
