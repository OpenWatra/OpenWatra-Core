// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Watra.Client.Web.ServerAccess;
    using Watra.Client.Web.ServerAccess.Services;
    using Watra.Client.Web.Tools.Mappings;

    /// <summary>
    /// ASP.Net Application startup routine.
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
        /// Gets the configuration, see <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // AutoMapper Config
            services.AddAutoMapper(typeof(Mappings));

            services.AddHttpClient<Api.Data.ApiClient.Client, Watra.Api.Data.ApiClient.Client>("ApiClient", (client) => { client.BaseAddress = new Uri(this.Configuration.GetSection("Api")["Endpoint"]); });

            services.AddScoped<IWatraRouteServerAccess, WatraRouteServerAccess>();
            services.AddScoped<IPumpSelectionServerAccess, PumpSelectionServerAccess>();
            services.AddScoped<IPumpServerAccess, PumpServerAccess>();
            services.AddScoped<IWatraCalculationServerAccess, WatraCalculationServerAccess>();
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
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
