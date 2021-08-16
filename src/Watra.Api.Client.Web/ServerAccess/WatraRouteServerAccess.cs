// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ServerAccess.Services;
    using Watra.Client.Web.ViewModel;

    /// <inheritdoc />
    public class WatraRouteServerAccess : IWatraRouteServerAccess
    {
        private readonly Client apiClient;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraRouteServerAccess"/> class.
        /// </summary>
        public WatraRouteServerAccess(Client apiClient, IMapper mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<WatraRoute> AddWatraRouteAsync(WatraRoute watraRoute)
        {
            return await this.apiClient.AddWatraRouteAsync(watraRoute);
        }

        /// <inheritdoc />
        public async Task<List<WatraRouteViewModel>> GetAllWatraRouteAsViewModelAsync()
        {
            var watras = await this.apiClient.WatraRouteAsync();

            return watras.Select(watra => this.mapper.Map<WatraRoute, WatraRouteViewModel>(watra)).ToList();
        }

        /// <inheritdoc />
        public Task GetAllWatraRouteAsync()
        {
            return Task.Factory.StartNew(() => this.apiClient.WatraRouteAsync());
        }

        /// <inheritdoc />
        public async Task<WatraRoute> GetWatraRouteByIdAsync(int id)
        {
            return await this.apiClient.GetWatraRouteByIdAsync(id);
        }

        /// <inheritdoc />
        public Task<WatraRoute> UpdateWatraRouteAsync(WatraRoute watraRoute)
        {
            return Task.Factory.StartNew(() => this.apiClient.UpdateWatraAsync(watraRoute)).Result;
        }

        /// <inheritdoc />
        public Task<ICollection<ValidationError>> ValidateWatraRouteAsync(WatraRoute watraRoute)
        {
            return this.apiClient.ValidateWatraRouteAsync(watraRoute);
        }

        /// <inheritdoc />
        public Task DeleteWatraRoute(int id)
        {
            return this.apiClient.DeleteWatraRouteAsync(id);
        }
    }
}
