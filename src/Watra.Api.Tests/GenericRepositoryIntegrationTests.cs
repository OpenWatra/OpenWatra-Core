// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Watra.Api.DataAccess;
    using Watra.Api.DataAccess.DbContext;
    using WaTra.Api.Server;
    using Watra.Api.Validation;

    /// <summary>
    /// DB tests for the <see cref="GenericRepository{TDbModel, TDto}"/>.
    /// </summary>
    [TestClass]
    public class GenericRepositoryIntegrationTests
    {
        private ServiceProvider databaseServiceProvider;

        /// <summary>
        /// Test initialization.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureDatabaseServices(serviceCollection, "Server=.\\;Database=WatraTestDb;Integrated Security=true");
            Startup.ConfigureAdditionalServices(serviceCollection);
            this.databaseServiceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = this.databaseServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<WatraContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Test cleanup.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = this.databaseServiceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<WatraContext>();
                context.Database.EnsureDeleted();
            }
        }

        /// <summary>
        /// Tests the CRUD operations on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbHose"/> as DB model, and a <see cref="Hose"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Hose_CRUD_WorksAsExpected()
        {
            // Arrange
            var repo = this.databaseServiceProvider.GetService<IGenericRepository<DbHose, Hose>>();

            // Act & Assert - CREATE
            var newConnector = new HoseConnector()
            {
                Name = "TestConnector",
                Diameter = 150,
            };

            var newHose = new Hose()
            {
                Name = "TestHose",
                ElementLengthInMetres = 30,
                HoseConnector = newConnector,
            };

            var createdHose = repo.Insert(newHose);

            Assert.AreEqual(createdHose.Id, 1);
            Assert.AreEqual(createdHose.Name, createdHose.Name);
            Assert.AreEqual(createdHose.ElementLengthInMetres, createdHose.ElementLengthInMetres);

            // Act & Assert - UPDATE
            createdHose.ElementLengthInMetres = 100.0;
            repo.Update(createdHose, createdHose.Id);

            // Act & Assert - READ
            var modifiedHose = repo.GetById(createdHose.Id);
            Assert.AreEqual(modifiedHose.ElementLengthInMetres, createdHose.ElementLengthInMetres);

            // Act & Assert - DELETE & READ all
            repo.Delete(modifiedHose.Id);

            var allHoses = repo.GetAll();
            Assert.AreEqual(allHoses.Count, 0);
        }

        /// <summary>
        /// Tests the CRUD operations on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbHoseConnector"/> as DB model, and a <see cref="HoseConnector"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void HoseConnector_CRUD_WorksAsExpected()
        {
            // Arrange
            var repo = this.databaseServiceProvider.GetService<IGenericRepository<DbHoseConnector, HoseConnector>>();

            // Act & Assert - CREATE
            var newConnector = new HoseConnector()
            {
                Name = "TestConnector",
                Diameter = 110,
            };

            var createdConnector = repo.Insert(newConnector);

            Assert.AreEqual(createdConnector.Id, 1);
            Assert.AreEqual(createdConnector.Name, newConnector.Name);
            Assert.AreEqual(createdConnector.Diameter, newConnector.Diameter);

            // Act & Assert - UPDATE
            createdConnector.Diameter = 99;
            repo.Update(createdConnector, createdConnector.Id);

            // Act & Assert - READ
            var modifiedConnector = repo.GetById(createdConnector.Id);
            Assert.AreEqual(modifiedConnector.Diameter, 99);

            // Act & Assert - DELETE & READ all
            repo.Delete(modifiedConnector.Id);

            var allConnectors = repo.GetAll();
            Assert.AreEqual(allConnectors.Count, 0);
        }

        /// <summary>
        /// Tests the CRUD operations on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbPump"/> as DB model, and a <see cref="Pump"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Pump_CRUD_WorksAsExpected()
        {
            // Arrange
            var repoPump = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();
            var repoFlowModel = this.databaseServiceProvider.GetService<IGenericRepository<DbPumpPressureFlowModelCoefficient, PumpPressureFlowModelCoefficient>>();

            var pressureFlowModel = new List<PumpPressureFlowModelCoefficient>()
            {
                new PumpPressureFlowModelCoefficient()
                {
                    Coefficient = 2.0,
                    Exponent = 0,
                },
                new PumpPressureFlowModelCoefficient()
                {
                    Coefficient = 4.0,
                    Exponent = 1,
                },
            };

            var pump = CreatePump(pressureFlowModel);

            // Act & Assert - CREATE
            var createdPump = repoPump.Insert(pump, pump => pump.PumpPressureFlowModel, pump => pump.Hose, pump => pump.HoseConnector);

            Assert.AreEqual(createdPump.Id, 1);

            var createdPressureFlowModel = createdPump.PumpPressureFlowModel;
            Assert.AreEqual(pressureFlowModel.Count, createdPressureFlowModel.Count);
            for (int i = 0; i < pressureFlowModel.Count; i++)
            {
                Assert.AreEqual(pressureFlowModel[i].Coefficient, createdPressureFlowModel[i].Coefficient);
                Assert.AreEqual(pressureFlowModel[i].Exponent, createdPressureFlowModel[i].Exponent);
            }

            Assert.AreEqual(pump.MaxFlowRateLitresPerMinute, createdPump.MaxFlowRateLitresPerMinute);
            Assert.AreEqual(pump.MaxOutletPressureBar, createdPump.MaxOutletPressureBar);
            Assert.AreEqual(pump.Name, createdPump.Name);

            // Act & Assert - UPDATE
            createdPump.MaxOutletPressureBar = 20;
            createdPressureFlowModel[0].Exponent = 2;
            var updatedPumpFromRepo = repoPump.Update(createdPump, createdPump.Id, pump => pump.PumpPressureFlowModel, pump => pump.HoseConnector, pump => pump.Hose);

            Assert.AreEqual(updatedPumpFromRepo.MaxOutletPressureBar, createdPump.MaxOutletPressureBar);

            var modifiedPressureFlowModel = updatedPumpFromRepo.PumpPressureFlowModel;
            Assert.AreEqual(modifiedPressureFlowModel.Count, createdPressureFlowModel.Count);
            for (int i = 0; i < pressureFlowModel.Count; i++)
            {
                // Entity framework is changing the PressureFlowModel IDs when updating the pump.
                // Either weird EF or auto mapper behavior.
                var created = createdPressureFlowModel[i];
                var modified = modifiedPressureFlowModel.Single(fm => fm.Exponent == created.Exponent);
                Assert.AreEqual(modified.Coefficient, created.Coefficient);
                Assert.AreEqual(modified.Exponent, created.Exponent);
            }

            // Act & Assert - DELETE & READ all
            repoPump.Delete(createdPump.Id);

            var allPumps = repoPump.GetAll();
            Assert.AreEqual(allPumps.Count, 0);

            var allFlowModels = repoFlowModel.GetAll();
            Assert.AreEqual(allFlowModels.Count, 0);
        }

        /// <summary>
        /// Tests the CRUD operations on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbPump"/> as DB model, and a <see cref="Pump"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Pump_Validation()
        {
            // Arrange
            var repoPump = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();

            var pump = new Pump()
            {
                MaxFlowRateLitresPerMinute = 2000.0,
                Name = null,
                MaxOutletPressureBar = -10,
                PumpPressureFlowModel = null,
                HoseConnector = new HoseConnector()
                {
                    Name = "Testanschluss",
                    Diameter = 110,
                },
                Hose = new Hose()
                {
                    ElementLengthInMetres = 100.0,
                    HoseConnector = new HoseConnector()
                    {
                        Name = "Testanschluss",
                        Diameter = 110,
                    },
                },
            };

            // Act & Assert - CREATE
            Assert.ThrowsException<WatraValidationException>(() => repoPump.Insert(pump));
        }

        /// <summary>
        /// Tests the validation operation (hoses and connector with different UniqueId) on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbPump"/> as DB model, and a <see cref="Pump"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Pump_Validation_UniqueId()
        {
            // Arrange
            var repoPump = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();

            var pump = new Pump()
            {
                MaxFlowRateLitresPerMinute = 2000.0,
                Name = "TestPumpe",
                MaxOutletPressureBar = 10,
                PumpPressureFlowModel = null,
                HoseConnector = new HoseConnector()
                {
                    Name = "Testanschluss",
                    Diameter = 110,
                    UniqueId = Guid.NewGuid().ToString(),
                },
                Hose = new Hose()
                {
                    ElementLengthInMetres = 100.0,
                    HoseConnector = new HoseConnector()
                    {
                        Name = "Testanschluss",
                        Diameter = 110,
                        UniqueId = Guid.NewGuid().ToString(),
                    },
                },
            };

            // Act & Assert - CREATE
            Assert.ThrowsException<WatraValidationException>(() => repoPump.Insert(pump));
        }

        /// <summary>
        /// Tests the validation operation (no hose type) on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbPump"/> as DB model, and a <see cref="Pump"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Pump_Validation_NoHose()
        {
            // Arrange
            var repoPump = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();

            var pump = new Pump()
            {
                MaxFlowRateLitresPerMinute = 2000.0,
                Name = "TestPumpe",
                MaxOutletPressureBar = 10,
                PumpPressureFlowModel = null,
                HoseConnector = null,
                Hose = new Hose()
                {
                    ElementLengthInMetres = 100.0,
                    HoseConnector = new HoseConnector()
                    {
                        Name = "Testanschluss",
                        Diameter = 110,
                    },
                },
            };

            // Act & Assert - CREATE
            Assert.ThrowsException<WatraValidationException>(() => repoPump.Insert(pump));
        }

        /// <summary>
        /// Tests the validation operation (no connecotr type) on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbPump"/> as DB model, and a <see cref="Pump"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void Pump_Validation_NoConnector()
        {
            // Arrange
            var repoPump = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();

            var pump = new Pump()
            {
                MaxFlowRateLitresPerMinute = 2000.0,
                Name = "TestPumpe",
                MaxOutletPressureBar = 10,
                PumpPressureFlowModel = null,
                HoseConnector = new HoseConnector()
                {
                    Name = "Testanschluss",
                    Diameter = 110,
                    UniqueId = Guid.NewGuid().ToString(),
                },
                Hose = null,
            };

            // Act & Assert - CREATE
            Assert.ThrowsException<WatraValidationException>(() => repoPump.Insert(pump));
        }

        /// <summary>
        /// Tests the CRUD operations on the <see cref="GenericRepository{TDbModel, TDto}"/>
        /// using a <see cref="DbHoseConnector"/> as DB model, and a <see cref="HoseConnector"/>
        /// as DTO.
        /// </summary>
        [TestMethod]
        public void WatraRoute_CRUD_WorksAsExpected()
        {
            // Arrange
            var repoWatra = this.databaseServiceProvider.GetService<IGenericRepository<DbWatraRoute, WatraRoute>>();
            var repoDistanceHeightElements = this.databaseServiceProvider.GetService<IGenericRepository<DbWatraRouteDistanceHeightElement, WatraRouteDistanceHeightElement>>();
            var repoPumpSelections = this.databaseServiceProvider.GetService<IGenericRepository<DbPumpSelection, PumpSelection>>();
            var repoPumps = this.databaseServiceProvider.GetService<IGenericRepository<DbPump, Pump>>();

            var distanceHeightElements = CreateDistanceHeightElements(5);
            var pressureFlowModel = new List<PumpPressureFlowModelCoefficient>()
            {
                new PumpPressureFlowModelCoefficient()
                {
                    Coefficient = 2.0,
                    Exponent = 0,
                },
                new PumpPressureFlowModelCoefficient()
                {
                    Coefficient = 4.0,
                    Exponent = 1,
                },
            };

            var pump1 = repoPumps.Insert(CreatePump(pressureFlowModel));
            var pump2 = repoPumps.Insert(CreatePump(pressureFlowModel));

            var pumpSelections = new List<PumpSelection>()
            {
                new PumpSelection()
                {
                    NumberOfHoseLines = 1,
                    Pump = pump1,
                },
                new PumpSelection()
                {
                    NumberOfHoseLines = 2,
                    Pump = pump2,
                },
            };

            var watraRoute = new WatraRoute()
            {
                Name = "TestRoute",
                Description = "Die Route führt über den Testberg nach Testhausen.",
                IsActiveWatra = false,
                FlowRate = 4000.0,
                MinimalOutletPressure = 2.0,
                SafetyPressure = 2.0,
                WatraRouteDistanceHeightElements = distanceHeightElements,
                PumpSelections = pumpSelections,
            };

            // Act & Assert - CREATE
            var createdWatraRoute = repoWatra.Insert(watraRoute);

            Assert.AreEqual(createdWatraRoute.Id, 1);
            Assert.AreEqual(createdWatraRoute.Name, watraRoute.Name);
            Assert.AreEqual(createdWatraRoute.Description, watraRoute.Description);
            Assert.AreEqual(createdWatraRoute.IsActiveWatra, watraRoute.IsActiveWatra);
            Assert.AreEqual(createdWatraRoute.FlowRate, watraRoute.FlowRate);
            Assert.AreEqual(createdWatraRoute.MinimalOutletPressure, watraRoute.MinimalOutletPressure);
            Assert.AreEqual(createdWatraRoute.PumpSelections.Count, watraRoute.PumpSelections.Count);

            foreach (var item in createdWatraRoute.PumpSelections)
            {
                var createdItem = createdWatraRoute.PumpSelections.Single(ps => ps.NumberOfHoseLines == item.NumberOfHoseLines);

                Assert.AreEqual(createdItem.Pump.Name, item.Pump.Name);
            }

            Assert.AreEqual(createdWatraRoute.WatraRouteDistanceHeightElements.Count, watraRoute.WatraRouteDistanceHeightElements.Count);

            foreach (var item in watraRoute.WatraRouteDistanceHeightElements)
            {
                var createdItem = createdWatraRoute.WatraRouteDistanceHeightElements.Single(he => he.HeightDifference == item.HeightDifference);

                Assert.AreEqual(createdItem.Length, item.Length);
            }

            // Act & Assert - UPDATE
            createdWatraRoute.IsActiveWatra = true;
            repoWatra.Update(createdWatraRoute, createdWatraRoute.Id);

            // Act & Assert - READ
            var modifiedWatraRoute = repoWatra.GetById(createdWatraRoute.Id);
            Assert.AreEqual(modifiedWatraRoute.IsActiveWatra, createdWatraRoute.IsActiveWatra);

            // Act & Assert - DELETE & READ all
            repoWatra.Delete(createdWatraRoute.Id);

            var allWatraRoutes = repoWatra.GetAll();
            Assert.AreEqual(allWatraRoutes.Count, 0);

            var allPumpSelections = repoPumpSelections.GetAll();
            Assert.AreEqual(allPumpSelections.Count, 0);

            var allDistanceHeightElements = repoDistanceHeightElements.GetAll();
            Assert.AreEqual(allDistanceHeightElements.Count, 0);
        }

        private static List<WatraRouteDistanceHeightElement> CreateDistanceHeightElements(int count)
        {
            var distanceHeightElements = new List<WatraRouteDistanceHeightElement>();
            Random random = new Random();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var element = new WatraRouteDistanceHeightElement()
                    {
                        Length = random.Next(100, 2000),
                        HeightDifference = random.Next(10, 200),
                    };
                    distanceHeightElements.Add(element);
                }
            }

            return distanceHeightElements;
        }

        private static Pump CreatePump(List<PumpPressureFlowModelCoefficient> pressureFlowModel = null)
        {
            var connector = new HoseConnector()
            {
                Name = "TestConnector",
                Diameter = 100,
            };

            var hose = new Hose()
            {
                Name = "Hose2",
                ElementLengthInMetres = 200.0,
                HoseConnector = connector,
            };

            var pump = new Pump()
            {
                Name = "TestPumpe1",
                HoseConnector = connector,
                Hose = hose,
                MaxFlowRateLitresPerMinute = 5000.0,
                MaxOutletPressureBar = 12,
                PumpPressureFlowModel = pressureFlowModel,
            };

            return pump;
        }
    }
}
