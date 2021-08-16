// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.Mappings
{
    using AutoMapper;
    using Watra.Api.DataAccess.DbContext;
    using WaTra.Api.Server;

    /// <summary>
    /// AutoMapper profile for mapping master data between database and API classes.
    /// </summary>
    public class MasterDataMapProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDataMapProfile"/> class.
        /// </summary>
        public MasterDataMapProfile()
        {
            // Hose Connector
            this.CreateMap<HoseConnector, DbHoseConnector>()
                .ReverseMap();

            // Hose
            this.CreateMap<Hose, DbHose>()
                .ReverseMap();

            // Pump
            this.CreateMap<Pump, DbPump>()
                .ReverseMap();

            this.CreateMap<PumpPressureFlowModelCoefficient, DbPumpPressureFlowModelCoefficient>()
                .ReverseMap();

            // WatraRoute
            this.CreateMap<WatraRoute, DbWatraRoute>()
                .ReverseMap();

            this.CreateMap<WatraRouteDistanceHeightElement, DbWatraRouteDistanceHeightElement>()
                .ReverseMap();

            this.CreateMap<PumpSelection, DbPumpSelection>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(selection => selection.Pump.Id))
                .ForMember(dest => dest.Pump, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
