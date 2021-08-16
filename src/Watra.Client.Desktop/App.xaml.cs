// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;
    using Watra.Client.Desktop.Navigation;
    using Watra.Client.Desktop.ServerAccess;
    using Watra.Client.Desktop.ViewModel;
    using Watra.Tools;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.host = Host.CreateDefaultBuilder()
               .ConfigureServices((context, services) =>
               {
                   this.ConfigureServices(context.Configuration, services);
               })
               .Build();
        }

        /// <summary>
        /// <see cref="Application.OnStartup(StartupEventArgs)"/>.
        /// </summary>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await this.host.StartAsync();

            var navHelper = this.host.Services.GetRequiredService<INavigationHelper>();
            navHelper.OpenMainWindow();
            base.OnStartup(e);
        }

        /// <summary>
        /// <see cref="Application.OnExit(ExitEventArgs)"/>.
        /// </summary>
        protected override async void OnExit(ExitEventArgs e)
        {
            using (this.host)
            {
                await this.host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            services.AddHttpClient<Api.Data.ApiClient.Client, Watra.Api.Data.ApiClient.Client>("ApiClient", (client) => { client.BaseAddress = new Uri(configuration.GetSection(nameof(AppSettings))["ApiEndpoint"]);  });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton(typeof(CrudViewModel<>));
            services.AddSingleton<CrudViewModel<Hose>, EditHoseViewModel>();
            services.AddSingleton<CrudViewModel<Pump>, EditPumpViewModel>();

            services.AddSingleton<IServerAccess<HoseConnector>, HoseConnectorServerAccess>();
            services.AddSingleton<IServerAccess<Hose>, HoseServerAccess>();
            services.AddSingleton<IServerAccess<Pump>, PumpServerAccess>();

            services.AddSingleton<INavigationHelper, NavigationHelper>();

            ServiceRegistrationUtils.RegisterServices(services);
        }
    }
}
