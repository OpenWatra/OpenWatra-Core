// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ServerAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;
    using Watra.Api.Data.DataAccess;

    /// <inheritdoc/>
    public class HoseServerAccess : IServerAccess<Hose>
    {
        private readonly Client apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoseServerAccess"/> class.
        /// </summary>
        public HoseServerAccess(Client apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(Hose model)
        {
            return this.apiClient.DeleteHoseAsync(model.Id);
        }

        /// <inheritdoc/>
        public Task<ICollection<Hose>> GetAll()
        {
            return this.apiClient.HoseAsync();
        }

        /// <inheritdoc/>
        public Task<Hose> UpdateAsync(Hose model)
        {
            if (model.Id > 0)
            {
                return this.apiClient.UpdateHoseAsync(model);
            }
            else
            {
                return this.apiClient.AddHoseAsync(model);
            }
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> Validate(Hose model)
        {
            return this.apiClient.ValidateHoseAsync(model);
        }
    }
}
