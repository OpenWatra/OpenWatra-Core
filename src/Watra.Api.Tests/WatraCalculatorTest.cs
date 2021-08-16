// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Watra.Api;
    using WaTra.Api.Server;

    /// <summary>
    /// Test class for the DistanceHeightCalculator class.
    /// </summary>
    [TestClass]
    public class WatraCalculatorTest
    {
        /// <summary>
        /// Test calculation of total route length.
        /// </summary>
        [TestMethod]
        public void TotalLength()
        {
            var testWatra = this.CreateTestWatra_HighWay66();
            var watraCalc = new WatraCalculator();

            var watraCalculation = watraCalc.CalculateWatra(testWatra);

            var sumDistance = 0.0;
            foreach (var section in watraCalculation.WatraCalculationSection)
            {
                sumDistance += section.SectionLength;
            }

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(testWatra.WatraRouteDistanceHeightElements);

            Assert.AreEqual(0, distHeightParam.TotalLength - sumDistance, watraCalc.AcceptedRemainingDistance);
        }

        /// <summary>
        /// Test calculation of total route length.
        /// The allowed difference of the section length of 200m is calculated from the suggested safety pressure of 2bar,
        /// the avarage slope on the test Watra and the pressure loss due to fricition from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband, i.e. 200m equal a pressure difference of 2bar.
        /// The expected positions were calcualted manually, using data from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband and the Excel sheet from Feuerwehr Zug.
        /// </summary>
        [TestMethod]
        public void SectionLengths()
        {
            var testWatra = this.CreateTestWatra_HighWay66();
            var watraCalc = new WatraCalculator();

            var watraCalculation = watraCalc.CalculateWatra(testWatra);

            Assert.AreEqual(1700, watraCalculation.WatraCalculationSection[0].SectionLength, 200);
            Assert.AreEqual(1700, watraCalculation.WatraCalculationSection[1].SectionLength, 200);
        }

        /// <summary>
        /// Teset watra that shoud finish successfully.
        /// </summary>
        private WatraRoute CreateTestWatra_HighWay66()
        {
            var connector110 = new HoseConnector()
            {
                Name = "Kupplung 110 mm",
                Diameter = 110,
            };

            var hose110 = new Hose()
            {
                Name = "Schlauch 110 mm",
                ElementLengthInMetres = 20,
                HoseConnector = connector110,
            };

            var pressureFlowModelType4_0 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = 14.4605,
                Exponent = 0,
            };

            var pressureFlowModelType4_1 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = 4.5261 * Math.Pow(10, -4),
                Exponent = 1,
            };

            var pressureFlowModelType4_2 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = -3.6979 * Math.Pow(10, -7),
                Exponent = 2,
            };

            var pressureFlowModelHydroSub_0 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = 12.3762,
                Exponent = 0,
            };

            var pressureFlowModelHydroSub_1 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = 0.0023,
                Exponent = 1,
            };

            var pressureFlowModelHydroSub_2 = new PumpPressureFlowModelCoefficient()
            {
                Coefficient = -7.8994 * Math.Pow(10, -7),
                Exponent = 2,
            };

            var pumpType4 = new Pump()
            {
                HoseConnector = connector110,
                MaxFlowRateLitresPerMinute = 2000,
                MaxOutletPressureBar = 12,
                Name = "Typ 4 [ZG 1234]",
                Hose = hose110,
                NumberOfHoses = 80,
                PumpPressureFlowModel = new List<PumpPressureFlowModelCoefficient>() { pressureFlowModelType4_0, pressureFlowModelType4_1, pressureFlowModelType4_2 },
            };

            var pumpHydroSub1 = new Pump()
            {
                HoseConnector = connector110,
                MaxFlowRateLitresPerMinute = 5000,
                MaxOutletPressureBar = 14.0,
                Name = "Hydro Sub [ZG 1181]",
                Hose = hose110,
                NumberOfHoses = 150,
                PumpPressureFlowModel = new List<PumpPressureFlowModelCoefficient>() { pressureFlowModelHydroSub_0, pressureFlowModelHydroSub_1, pressureFlowModelHydroSub_2 },
            };

            var pumpHydroSub2 = new Pump()
            {
                HoseConnector = connector110,
                MaxFlowRateLitresPerMinute = 5000,
                MaxOutletPressureBar = 14.0,
                Name = "Hydro Sub [ZG 1182]",
                Hose = hose110,
                NumberOfHoses = 150,
                PumpPressureFlowModel = new List<PumpPressureFlowModelCoefficient>() { pressureFlowModelHydroSub_0, pressureFlowModelHydroSub_1, pressureFlowModelHydroSub_2 },
            };

            // TestWatra
            var testWatra = new WatraRoute();

            testWatra.PumpSelections = new List<PumpSelection>();
            testWatra.PumpSelections.Add(new PumpSelection()
            {
                Pump = pumpHydroSub1,
                NumberOfHoseLines = 2,
                SortOrder = 1,
            });
            testWatra.PumpSelections.Add(new PumpSelection()
            {
                Pump = pumpHydroSub2,
                NumberOfHoseLines = 2,
                SortOrder = 2,
            });
            testWatra.PumpSelections.Add(new PumpSelection()
            {
                Pump = pumpType4,
                NumberOfHoseLines = 2,
                SortOrder = 3,
            });

            testWatra.WatraRouteDistanceHeightElements = new List<WatraRouteDistanceHeightElement>()
                {
                    new WatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = 50,
                        SortOrder = 1,
                    },
                    new WatraRouteDistanceHeightElement()
                    {
                        Length = 500,
                        HeightDifference = -50,
                        SortOrder = 2,
                    },
                    new WatraRouteDistanceHeightElement()
                    {
                        Length = 1000,
                        HeightDifference = 80,
                        SortOrder = 3,
                    },
                    new WatraRouteDistanceHeightElement()
                    {
                        Length = 600,
                        HeightDifference = -20,
                        SortOrder = 4,
                    },
                    new WatraRouteDistanceHeightElement()
                    {
                        Length = 800,
                        HeightDifference = 40,
                        SortOrder = 5,
                    },
                };

            testWatra.Name = "TestWatra: HighWay66";
            testWatra.Description = "Watra wird erfolgreich berechnet.";
            testWatra.MinimalOutletPressure = 2.0;
            testWatra.SafetyPressure = 2.0;
            testWatra.IsActiveWatra = true;
            testWatra.FlowRate = 2000;

            return testWatra;
        }
    }
}
