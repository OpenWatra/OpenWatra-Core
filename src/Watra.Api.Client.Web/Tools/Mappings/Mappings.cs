// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.Tools.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ViewModel;

    /// <summary>
    /// Mappings for auto mapper.
    /// </summary>
    public class Mappings : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mappings"/> class.
        /// </summary>
        public Mappings()
        {
            // WatraRouteCreateViewModel
            this.CreateMap<WatraRouteCreateViewModel, WatraRoute>()
                .ForMember(route => route.PumpSelections, config => config.MapFrom(viewmodel => viewmodel.PumpSelections.Where(selection => selection.IsSelected)));

            // WatraRouteHeightDistanceElementViewModel
            this.CreateMap<WatraRouteHeightDistanceElementViewModel, WatraRouteDistanceHeightElement>()
                .ReverseMap();

            // WatraRouteViewModel
            this.CreateMap<WatraRoute, WatraRouteViewModel>()
                .ForMember(viewModel => viewModel.Id, config => config.MapFrom(watra => watra.Id))
                .ForMember(viewModel => viewModel.Name, config => config.MapFrom(watra => watra.Name))
                .ForMember(viewModel => viewModel.Description, config => config.MapFrom(watra => watra.Description))
                .ForMember(viewModel => viewModel.IsActiveWatra, config => config.MapFrom(watra => watra.IsActiveWatra));

            // WatraCalcViewModel
            this.CreateMap<WatraCalculation, WatraCalculationViewModel>()
                .ForMember(viewModel => viewModel.Name, config => config.MapFrom(calc => calc.Name))
                .ForMember(viewModel => viewModel.Description, config => config.MapFrom(watra => watra.Description))
                .ForMember(viewModel => viewModel.SafetyPressure, config => config.MapFrom(calc => calc.SafetyPressure))
                .ForMember(viewModel => viewModel.ActualFlowRate, config => config.MapFrom(calc => calc.ActualFlowRate))
                .ForMember(viewModel => viewModel.RemainingDistance, config => config.MapFrom(calc => calc.RemainingDistance));

            this.CreateMap<ICollection<WatraCalculationSection>, List<WatraCalculationSectionViewModel>>();
            this.CreateMap<WatraCalculationSection, WatraCalculationSectionViewModel>()
                .ForMember(viewModel => viewModel.DiameterConnector, config => config.MapFrom(section => section.DiameterConnector))
                .ForMember(viewModel => viewModel.NameOfPump, config => config.MapFrom(section => section.NameOfPump));

            this.CreateMap<ICollection<WatraCalculationMessage>, List<WatraCalculationMessageViewModel>>();
            this.CreateMap<WatraCalculationMessage, WatraCalculationMessageViewModel>()
                .ForMember(viewModel => viewModel.Severity, config => config.MapFrom(section => section.Severity))
                .ForMember(viewModel => viewModel.Message, config => config.MapFrom(section => section.Message));

            // WatraRouteCreateViewModel
            this.CreateMap<WatraRouteEditViewModel, WatraRoute>()
                .ReverseMap();

            // WatraRouteDeleteViewModel
            this.CreateMap<WatraRouteDeleteViewModel, WatraRoute>()
                .ReverseMap();

            // Pump
            this.CreateMap<Pump, PumpSelectionViewModel>()
                .ForMember(viewModel => viewModel.IdPump, config => config.MapFrom(pump => pump.Id))
                .ForMember(viewModel => viewModel.Name, config => config.MapFrom(pump => pump.Name))
                .ForMember(viewModel => viewModel.IsSelected, config => config.MapFrom(pump => true))
                .ForMember(viewModel => viewModel.Pump, config => config.MapFrom(pump => pump));

            // PumpSelectionViewModel
            this.CreateMap<PumpSelectionViewModel, PumpSelection>()
                .ForMember(pumpSelection => pumpSelection.SortOrder, config => config.MapFrom(viewModel => viewModel.SortOrder))
                .ForMember(pumpSelection => pumpSelection.Pump, config => config.MapFrom(viewModel => viewModel.Pump))
                .ForMember(pumpSelection => pumpSelection.NumberOfHoseLines, config => config.MapFrom(viewModel => 2));
        }
    }
}
