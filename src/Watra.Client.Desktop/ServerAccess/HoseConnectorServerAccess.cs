// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ServerAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;

    /// <inheritdoc/>
    public class HoseConnectorServerAccess : IServerAccess<HoseConnector>
    {
        private readonly Client apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoseConnectorServerAccess"/> class.
        /// </summary>
        public HoseConnectorServerAccess(Client apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <inheritdoc/>
        public Task<ICollection<HoseConnector>> GetAll()
        {
            return this.apiClient.HoseConnectorAsync();
        }

        /// <inheritdoc/>
        public Task DeleteAsync(HoseConnector model)
        {
            return this.apiClient.DeleteHoseConnectorAsync(model.Id);
        }

        /// <inheritdoc/>
        public Task<HoseConnector> UpdateAsync(HoseConnector model)
        {
            if (model.Id > 0)
            {
                return this.apiClient.UpdateHoseConnectorAsync(model);
            }
            else
            {
                return this.apiClient.AddHoseConnectorAsync(model);
            }
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> Validate(HoseConnector model)
        {
            return this.apiClient.ValidateHoseConnectorAsync(model);
        }
    }
}
