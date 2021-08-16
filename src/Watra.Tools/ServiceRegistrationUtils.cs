// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Utilities to register services within this project.
    /// </summary>
    public static class ServiceRegistrationUtils
    {
        /// <summary>
        /// Registers services to the <paramref name="services"/>.
        /// </summary>
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IInstanceCreator<>), typeof(InstanceCreator<>));
        }
    }
}
