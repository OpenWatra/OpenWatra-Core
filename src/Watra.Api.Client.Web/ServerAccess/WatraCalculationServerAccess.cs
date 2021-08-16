// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ViewModel;

    /// <inheritdoc />
    public class WatraCalculationServerAccess : IWatraCalculationServerAccess
    {
        private readonly Client apiClient;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraCalculationServerAccess"/> class.
        /// </summary>
        public WatraCalculationServerAccess(Client apiClient, IMapper mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<WatraCalculationViewModel> GetWatraCalculationByIdAsyncAsViewModel(int id)
        {
            var calculation = await this.apiClient.GetWatraRouteCalculationAsync(id);

            // As per SO, mapping of entities explicitly done:
            // https://stackoverflow.com/questions/13479208/automapper-auto-map-collection-property-for-a-dto-object/13499361
            var mappedCalc = this.mapper.Map<WatraCalculation, WatraCalculationViewModel>(calculation);
            var mappedSections = calculation.WatraCalculationSection.Select(watra => this.mapper.Map<WatraCalculationSection, WatraCalculationSectionViewModel>(watra)).ToList();
            var mappedMessages = calculation.WatraCalculationMessage.Select(watra => this.mapper.Map<WatraCalculationMessage, WatraCalculationMessageViewModel>(watra)).ToList();

            mappedCalc.WatraCalculationSection = mappedSections;
            mappedCalc.WatraCalculationMessage = mappedMessages;

            return mappedCalc;
        }

        /// <inheritdoc />
        public Task<WatraCalculation> GetWatraCalculationByIdAsync(int id)
        {
            return this.apiClient.GetWatraRouteCalculationAsync(id);
        }
    }
}
