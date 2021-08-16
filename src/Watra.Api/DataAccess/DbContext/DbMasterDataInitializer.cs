// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Initializer class for master data on the database.
    /// </summary>
    public static class DbMasterDataInitializer
    {
        /// <summary>
        /// Initialize the database with master data.
        /// </summary>
        public static void Initialize(WatraContext watraContext)
        {
            if (!watraContext.HoseConnectors.Any())
            {
                var connector55 = new DbHoseConnector()
                {
                    Name = "Kupplung 55 mm",
                    Diameter = 55,
                    UniqueId = Guid.NewGuid(),
                };

                var connector110 = new DbHoseConnector()
                {
                    Name = "Kupplung 110 mm",
                    Diameter = 110,
                    UniqueId = Guid.NewGuid(),
                };

                var connector75 = new DbHoseConnector()
                {
                    Name = "Kupplung 75 mm",
                    Diameter = 75,
                    UniqueId = Guid.NewGuid(),
                };

                var hose110 = new DbHose()
                {
                    Name = "Schlauch 110 mm",
                    ElementLengthInMetres = 20,
                    HoseConnector = connector110,
                };

                var hose75 = new DbHose()
                {
                    Name = "Schlauch 75 mm",
                    ElementLengthInMetres = 20,
                    HoseConnector = connector75,
                };

                var hose55 = new DbHose()
                {
                    Name = "Schlauch 55 mm",
                    ElementLengthInMetres = 20,
                    HoseConnector = connector55,
                };

                var pressureFlowModelType4_0 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = 14.4605,
                    Exponent = 0,
                };

                var pressureFlowModelType4_1 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = 4.5261 * Math.Pow(10, -4),
                    Exponent = 1,
                };

                var pressureFlowModelType4_2 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = -3.6979 * Math.Pow(10, -7),
                    Exponent = 2,
                };

                var pressureFlowModelHydroSub_0 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = 12.3762,
                    Exponent = 0,
                };

                var pressureFlowModelHydroSub_1 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = 0.0023,
                    Exponent = 1,
                };

                var pressureFlowModelHydroSub_2 = new DbPumpPressureFlowModelCoefficient()
                {
                    Coefficient = -7.8994 * Math.Pow(10, -7),
                    Exponent = 2,
                };

                var pumpType4_1 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 2000,
                    MaxOutletPressureBar = 12,
                    Name = "Typ 4 [ZG 1234]",
                    Hose = hose110,
                    NumberOfHoses = 80,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelType4_0, pressureFlowModelType4_1, pressureFlowModelType4_2 },
                };

                var pumpType4_2 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 2000,
                    MaxOutletPressureBar = 12,
                    Name = "Typ 4 [ZG 5678]",
                    Hose = hose110,
                    NumberOfHoses = 10,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelType4_0, pressureFlowModelType4_1, pressureFlowModelType4_2 },
                };

                var pumpType4_3 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 2000,
                    MaxOutletPressureBar = 12,
                    Name = "Typ 4 [ZG 91011]",
                    Hose = hose110,
                    NumberOfHoses = 80,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelType4_0, pressureFlowModelType4_1, pressureFlowModelType4_2 },
                };

                var pumpHydroSub1 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 5000,
                    MaxOutletPressureBar = 14.0,
                    Name = "Hydro Sub [ZG 1181]",
                    Hose = hose110,
                    NumberOfHoses = 150,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelHydroSub_0, pressureFlowModelHydroSub_1, pressureFlowModelHydroSub_2 },
                };

                var pumpHydroSub2 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 5000,
                    MaxOutletPressureBar = 14.0,
                    Name = "Hydro Sub [ZG 1182]",
                    Hose = hose110,
                    NumberOfHoses = 150,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelHydroSub_0, pressureFlowModelHydroSub_1, pressureFlowModelHydroSub_2 },
                };

                var pumpHydroSub3 = new DbPump()
                {
                    HoseConnector = connector110,
                    MaxFlowRateLitresPerMinute = 5000,
                    MaxOutletPressureBar = 14.0,
                    Name = "Hydro Sub [ZG 1183]",
                    Hose = hose110,
                    NumberOfHoses = 5,
                    PumpPressureFlowModel = new List<DbPumpPressureFlowModelCoefficient>() { pressureFlowModelHydroSub_0, pressureFlowModelHydroSub_1, pressureFlowModelHydroSub_2 },
                };

                // TestWatra 1
                var testWatra = new DbWatraRoute();

                testWatra.PumpSelections = new List<DbPumpSelection>();
                testWatra.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub1,
                    NumberOfHoseLines = 2,
                    SortOrder = 1,
                });
                testWatra.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub2,
                    NumberOfHoseLines = 2,
                    SortOrder = 2,
                });
                testWatra.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpType4_1,
                    NumberOfHoseLines = 2,
                    SortOrder = 3,
                });

                testWatra.WatraRouteDistanceHeightElements = new List<DbWatraRouteDistanceHeightElement>()
                {
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = 50,
                        SortOrder = 1,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = -50,
                        SortOrder = 2,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 1000,
                        HeightDifference = 80,
                        SortOrder = 3,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 600,
                        HeightDifference = -20,
                        SortOrder = 4,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 800,
                        HeightDifference = 40,
                        SortOrder = 5,
                    },
                };

                testWatra.Name = "TestWatra: HighWay66";
                testWatra.Description = "Watra wird erfolgreich berechnet (eine Pumpe zuviel ausgewählt).";
                testWatra.MinimalOutletPressure = 2.0;
                testWatra.SafetyPressure = 2.0;
                testWatra.IsActiveWatra = true;
                testWatra.FlowRate = 2000;

                watraContext.Add(testWatra);

                // TestWatra 2
                var testWatra2 = new DbWatraRoute();

                testWatra2.PumpSelections = new List<DbPumpSelection>();
                testWatra2.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub1,
                    NumberOfHoseLines = 2,
                    SortOrder = 1,
                });
                testWatra2.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub3,
                    NumberOfHoseLines = 2,
                    SortOrder = 2,
                });

                testWatra2.WatraRouteDistanceHeightElements = new List<DbWatraRouteDistanceHeightElement>()
                {
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = 50,
                        SortOrder = 1,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = -50,
                        SortOrder = 2,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 1000,
                        HeightDifference = 80,
                        SortOrder = 3,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 600,
                        HeightDifference = -20,
                        SortOrder = 4,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 800,
                        HeightDifference = 40,
                        SortOrder = 5,
                    },
                };

                testWatra2.Name = "TestWatra: HighWay66";
                testWatra2.Description = "Auf einer Pumpe sind zu wenig Schläuche.";
                testWatra2.MinimalOutletPressure = 2.0;
                testWatra2.SafetyPressure = 2.0;
                testWatra2.IsActiveWatra = false;
                testWatra2.FlowRate = 2000;

                watraContext.Add(testWatra2);

                // TestWatra 3
                var testWatra3 = new DbWatraRoute();

                testWatra3.PumpSelections = new List<DbPumpSelection>();
                testWatra3.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub1,
                    NumberOfHoseLines = 2,
                    SortOrder = 1,
                });
                testWatra3.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpType4_1,
                    NumberOfHoseLines = 2,
                    SortOrder = 3,
                });

                testWatra3.WatraRouteDistanceHeightElements = new List<DbWatraRouteDistanceHeightElement>()
                {
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = 50,
                        SortOrder = 1,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = -50,
                        SortOrder = 2,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 1000,
                        HeightDifference = 80,
                        SortOrder = 3,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 600,
                        HeightDifference = -20,
                        SortOrder = 4,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 800,
                        HeightDifference = 40,
                        SortOrder = 5,
                    },
                };

                testWatra3.Name = "TestWatra: HighWay66";
                testWatra3.Description = "Maximale Durchflussrate wird limitiert.";
                testWatra3.MinimalOutletPressure = 2.0;
                testWatra3.SafetyPressure = 2.0;
                testWatra3.IsActiveWatra = false;
                testWatra3.FlowRate = 3000;

                watraContext.Add(testWatra3);

                // TestWatra 4
                var testWatra4 = new DbWatraRoute();

                testWatra4.PumpSelections = new List<DbPumpSelection>();
                testWatra4.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub1,
                    NumberOfHoseLines = 2,
                    SortOrder = 1,
                });
                testWatra4.PumpSelections.Add(new DbPumpSelection()
                {
                    Pump = pumpHydroSub2,
                    NumberOfHoseLines = 2,
                    SortOrder = 2,
                });

                testWatra4.WatraRouteDistanceHeightElements = new List<DbWatraRouteDistanceHeightElement>()
                {
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = 50,
                        SortOrder = 1,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = -50,
                        SortOrder = 2,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 1000,
                        HeightDifference = 80,
                        SortOrder = 3,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 600,
                        HeightDifference = -20,
                        SortOrder = 4,
                    },
                    new DbWatraRouteDistanceHeightElement()
                    {
                        Length = 2000,
                        HeightDifference = 80,
                        SortOrder = 5,
                    },
                };

                testWatra4.Name = "TestWatra: HighWay66";
                testWatra4.Description = "Nicht genügend Pumpen eingeplant.";
                testWatra4.MinimalOutletPressure = 2.0;
                testWatra4.SafetyPressure = 2.0;
                testWatra4.IsActiveWatra = false;
                testWatra4.FlowRate = 2000;

                watraContext.Add(testWatra4);

                // end temporary
                watraContext.Add(connector110);
                watraContext.Add(connector75);
                watraContext.Add(connector55);
                watraContext.Add(hose110);
                watraContext.Add(hose75);
                watraContext.Add(hose55);
                watraContext.Add(pumpType4_1);
                watraContext.Add(pumpType4_2);
                watraContext.Add(pumpType4_3);
                watraContext.Add(pumpHydroSub1);
                watraContext.Add(pumpHydroSub2);
                watraContext.SaveChanges();
            }
        }
    }
}
