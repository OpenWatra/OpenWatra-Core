// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;

    /// <inheritdoc />
    public class PumpServerAccess : IPumpServerAccess
    {
        private readonly Client apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpServerAccess"/> class.
        /// </summary>
        public PumpServerAccess(Client apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <inheritdoc />
        public Task<Pump> GetPumpByIdAsync(int id)
        {
            return this.apiClient.GetPumpByIdAsync(id);
        }
    }
}
