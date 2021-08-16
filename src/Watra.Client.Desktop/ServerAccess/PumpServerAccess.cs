// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ServerAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;

    /// <inheritdoc/>
    public class PumpServerAccess : IServerAccess<Pump>
    {
        private readonly Client apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpServerAccess"/> class.
        /// </summary>
        public PumpServerAccess(Client apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(Pump model)
        {
            return this.apiClient.DeletePumpAsync(model.Id);
        }

        /// <inheritdoc/>
        public Task<ICollection<Pump>> GetAll()
        {
            return this.apiClient.PumpAsync();
        }

        /// <inheritdoc/>
        public Task<Pump> UpdateAsync(Pump model)
        {
            if (model.Id > 0)
            {
                return this.apiClient.UpdatePumpAsync(model);
            }
            else
            {
                return this.apiClient.AddPumpAsync(model);
            }
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> Validate(Pump model)
        {
            return this.apiClient.ValidatePumpAsync(model);
        }
    }
}
