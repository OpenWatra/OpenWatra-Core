// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.DataAccess;
    using Watra.Api.DataAccess.DbContext;
    using WaTra.Api.Server;
    using Watra.Api.Validation;

    /// <summary>
    /// Implementation of the <see cref="IWatraMasterDataApiController"/> as database access.
    /// </summary>
    public class WatraMasterDataApiDbAccess : IWatraMasterDataApiController
    {
        private readonly IGenericRepository<DbHose, Hose> hoseRepository;
        private readonly IGenericRepository<DbHoseConnector, HoseConnector> hoseConnectorRepository;
        private readonly IGenericRepository<DbPump, Pump> pumpRepository;
        private readonly IGenericRepository<DbWatraRoute, WatraRoute> watraRouteRepository;
        private readonly IGenericRepository<DbPumpPressureFlowModelCoefficient, PumpPressureFlowModelCoefficient> pumpPressureFlowModelCoefficientRepository;

        private readonly IValidator<Hose> hoseValidator;
        private readonly IValidator<HoseConnector> hoseConnectorValidator;
        private readonly IValidator<Pump> pumpValidator;
        private readonly IValidator<WatraRoute> watraRouteValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatraMasterDataApiDbAccess"/> class.
        /// </summary>
        public WatraMasterDataApiDbAccess(
            IGenericRepository<DbHose, Hose> hoseRepository,
            IGenericRepository<DbHoseConnector, HoseConnector> hoseConnectorRepository,
            IGenericRepository<DbPump, Pump> pumpRepository,
            IGenericRepository<DbWatraRoute, WatraRoute> watraRouteRepository,
            IValidator<Hose> hoseValidator,
            IValidator<HoseConnector> hoseConnectorValidator,
            IValidator<Pump> pumpValidator,
            IValidator<WatraRoute> watraRouteValidator,
            IGenericRepository<DbPumpPressureFlowModelCoefficient, PumpPressureFlowModelCoefficient> pumpPressureFlowModelCoefficientRepository)
        {
            this.hoseRepository = hoseRepository;
            this.hoseConnectorRepository = hoseConnectorRepository;
            this.pumpRepository = pumpRepository;
            this.watraRouteRepository = watraRouteRepository;

            this.hoseValidator = hoseValidator;
            this.hoseConnectorValidator = hoseConnectorValidator;
            this.pumpValidator = pumpValidator;
            this.watraRouteValidator = watraRouteValidator;
            this.pumpPressureFlowModelCoefficientRepository = pumpPressureFlowModelCoefficientRepository;
        }

        /// <inheritdoc/>
        public Task<Hose> AddHoseAsync(Hose body)
        {
            return Task.Factory.StartNew(() =>
            {
                return this.hoseRepository.Insert(body);
            });
        }

        /// <inheritdoc/>
        public Task<HoseConnector> AddHoseConnectorAsync(HoseConnector body)
        {
            // Guid must be set, as it is not changeable on client side.
            body.UniqueId = Guid.NewGuid().ToString();

            return Task.Factory.StartNew(() =>
            {
                return this.hoseConnectorRepository.Insert(body);
            });
        }

        /// <inheritdoc/>
        public Task<Pump> AddPumpAsync(Pump body)
        {
            return Task.Factory.StartNew(() =>
            {
                return this.pumpRepository.Update(body, body.Id);
            });
        }

        /// <inheritdoc/>
        public Task<WatraRoute> AddWatraRouteAsync(WatraRoute body)
        {
            return Task.Factory.StartNew(() =>
            {
                var createdWatraRoute = this.watraRouteRepository.Insert(body);
                return createdWatraRoute;
            });
        }

        /// <inheritdoc/>
        public Task DeleteHoseAsync(int id)
        {
            this.hoseRepository.Delete(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteHoseConnectorAsync(int id)
        {
            this.hoseConnectorRepository.Delete(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeletePumpAsync(int id)
        {
            this.pumpRepository.Delete(id);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteWatraRouteAsync(int id)
        {
            this.watraRouteRepository.Delete(id);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<Hose> GetHoseByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => this.hoseRepository.GetById(id));
        }

        /// <inheritdoc/>
        public Task<HoseConnector> GetHoseConnectorByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => this.hoseConnectorRepository.GetById(id));
        }

        /// <inheritdoc/>
        public Task<Pump> GetPumpByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => this.pumpRepository.GetById(id));
        }

        /// <inheritdoc/>
        public Task<WatraRoute> GetWatraRouteByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => this.watraRouteRepository.GetById(id));
        }

        /// <inheritdoc/>
        public Task<WatraCalculation> GetWatraRouteCalculationAsync(int id)
        {
            return Task.Factory.StartNew(() =>
            {
                var watraRoute = this.watraRouteRepository.GetById(id);
                if (watraRoute == null)
                {
                    throw new System.NotImplementedException();
                }

                var watraCalulator = new WatraCalculator();
                return watraCalulator.CalculateWatra(watraRoute);
            });
        }

        /// <inheritdoc/>
        public Task<ICollection<Hose>> HoseAsync()
        {
            return Task.Factory.StartNew(() => this.hoseRepository.GetAll(hose => hose.HoseConnector));
        }

        /// <inheritdoc/>
        public Task<ICollection<HoseConnector>> HoseConnectorAsync()
        {
            return Task.Factory.StartNew(() => this.hoseConnectorRepository.GetAll());
        }

        /// <inheritdoc/>
        public Task<ICollection<Pump>> PumpAsync()
        {
            return Task.Factory.StartNew(() => this.pumpRepository.GetAll(pump => pump.Hose, pump => pump.HoseConnector));
        }

        /// <inheritdoc/>
        public Task<Hose> UpdateHoseAsync(Hose body)
        {
            return Task.Factory.StartNew(() => this.hoseRepository.Update(body, body.Id));
        }

        /// <inheritdoc/>
        public Task<HoseConnector> UpdateHoseConnectorAsync(HoseConnector body)
        {
            return Task.Factory.StartNew(() => this.hoseConnectorRepository.Update(body, body.Id));
        }

        /// <inheritdoc/>
        public Task<Pump> UpdatePumpAsync(Pump body)
        {
            return Task.Factory.StartNew(() =>
            {
                // Because Pump Pressure Flow Models are edited within the UI and not saved separately
                // this means that we must "sync" the DB data with what's being updated.
                // ToDo GH-36
                var pump = this.pumpRepository.GetById(body.Id);
                foreach (var flowModelInDb in pump.PumpPressureFlowModel)
                {
                    if (body.PumpPressureFlowModel.Find(flowModel => flowModel.Id == flowModelInDb.Id) == null)
                    {
                        this.pumpPressureFlowModelCoefficientRepository.Delete(flowModelInDb.Id);
                    }
                }

                return this.pumpRepository.Update(body, body.Id);
            });
        }

        /// <inheritdoc/>
        public Task<WatraRoute> UpdateWatraAsync(WatraRoute body)
        {
            return Task.Factory.StartNew(() => this.watraRouteRepository.Update(body, body.Id));
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> ValidateHoseAsync(Hose body)
        {
            return Task.Factory.StartNew(() => this.hoseValidator.Validate(body));
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> ValidateHoseConnectorAsync(HoseConnector body)
        {
            return Task.Factory.StartNew(() => this.hoseConnectorValidator.Validate(body));
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> ValidatePumpAsync(Pump body)
        {
            return Task.Factory.StartNew(() => this.pumpValidator.Validate(body));
        }

        /// <inheritdoc/>
        public Task<ICollection<ValidationError>> ValidateWatraRouteAsync(WatraRoute body)
        {
            return Task.Factory.StartNew(() => this.watraRouteValidator.Validate(body));
        }

        /// <inheritdoc/>
        public Task<ICollection<WatraRoute>> WatraRouteAsync()
        {
            return Task.Factory.StartNew(() => this.watraRouteRepository.GetAll());
        }
    }
}
