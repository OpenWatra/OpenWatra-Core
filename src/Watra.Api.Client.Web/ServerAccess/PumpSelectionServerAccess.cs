// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ViewModel;

    /// <inheritdoc />
    public class PumpSelectionServerAccess : IPumpSelectionServerAccess
    {
        private readonly Client apiClient;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpSelectionServerAccess"/> class.
        /// </summary>
        public PumpSelectionServerAccess(Client apiClient, IMapper mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<PumpSelectionViewModel>> GetPossiblePumpSelectionsAsViewModelsAsync()
        {
            var pumps = await this.apiClient.PumpAsync();

            return pumps.Select(pump => this.mapper.Map<Pump, PumpSelectionViewModel>(pump)).ToList();
        }
    }
}
