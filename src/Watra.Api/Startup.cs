// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Watra.Api.Controllers;
    using Watra.Api.DataAccess;
    using Watra.Api.DataAccess.DbContext;
    using Watra.Api.DataAccess.Mappings;
    using WaTra.Api.Server;
    using Watra.Api.Validation;

    /// <summary>
    /// Start up logic.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures additional custom services.
        /// </summary>
        public static void ConfigureAdditionalServices(IServiceCollection services)
        {
            // Services
            services.AddScoped<IWatraMasterDataApiController, WatraMasterDataApiDbAccess>();

            // Validators
            services.AddScoped(typeof(IValidator<>), typeof(DoNothingValidator<>));
            services.AddScoped<IValidator<Hose>, HoseValidator>();
            services.AddScoped<IValidator<HoseConnector>, HoseConnectorValidator>();
            services.AddScoped<IValidator<Pump>, PumpValidator>();
            services.AddScoped<IValidator<WatraRoute>, WatraRouteValidator>();

            // External Services
            Tools.ServiceRegistrationUtils.RegisterServices(services);
        }

        /// <summary>
        /// Adds all services needed to access the database to the <paramref name="services"/>.
        /// </summary>
        public static void ConfigureDatabaseServices(IServiceCollection services, string connectionString)
        {
            // AutoMapper Config
            services.AddAutoMapper(typeof(MasterDataMapProfile));

            // Entity Framework
            services.AddDbContext<WatraContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Generic repository
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Controllers
            services.AddControllers();

            // Database
            ConfigureDatabaseServices(services, this.Configuration.GetConnectionString("WatraContext"));

            ConfigureAdditionalServices(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
