// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WaTra.Api.Server;
    using Watra.Tools;

    /// <summary>
    /// Can calculate a WatraCalculation from a given DbWatraRoute.
    /// </summary>
    public class WatraCalculator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatraCalculator"/> class.
        /// </summary>
        /// <param name="recommendedMinOutletPressure">Recommended minimal outlet pressure at the end of a Watra (standard value: 2.0 bar), Unit: bar</param>
        /// <param name="recommendedSafetyPressure">Safety pressure used during calculation (standard value: 2.0 bar), Unit: bar</param>
        /// <param name="acceptedRemainingDistance">Accepted remaining distance at the end of a Watra (standard value: 20m), Unit: m</param>
        public WatraCalculator(double recommendedMinOutletPressure = 2.0, double recommendedSafetyPressure = 2.0, double acceptedRemainingDistance = 20.0)
        {
            this.RecommendedMinOutletPressure = recommendedMinOutletPressure;
            this.RecommendedSafetyPressure = recommendedSafetyPressure;
            this.AcceptedRemainingDistance = acceptedRemainingDistance;
        }

        /// <summary>
        /// Gets recommended minimal outlet pressure at the end of a Watra, Unit: bar
        /// </summary>
        public double RecommendedMinOutletPressure { get; }

        /// <summary>
        /// Gets safety pressure used during calculation (standard value: 2.0 bar), Unit: bar
        /// </summary>
        public double RecommendedSafetyPressure { get; }

        /// <summary>
        /// Gets accepted remaining distance at the end of a Watra (standard value: 20m), Unit: m
        /// </summary>
        public double AcceptedRemainingDistance { get; }

        /// <summary>
        /// Method that can calculate a WatraCalculation from a given DbWatraRoute.
        /// </summary>
        /// <param name="watraIn">Watra from database.</param>
        /// <returns>Calculated Watra</returns>
        public WatraCalculation CalculateWatra(WatraRoute watraIn)
        {
            // initalize caluclators
            var distHeightCalc = new DistanceHeightCalculator();
            var hydCalc = new HydrodynamicCalculator(20);

            // load watra parameters
            var distHeightCalcParam = new DistanceHeightCalculationParameters();
            distHeightCalcParam.AddNewWatraElements(watraIn.WatraRouteDistanceHeightElements);
            var pumpSelections = watraIn.PumpSelections;
            var flowRateIn = watraIn.FlowRate;
            var minimalOutletPressure = watraIn.MinimalOutletPressure;
            var pressureSafety = watraIn.SafetyPressure;

            // calculate new WatraElement
            var caluclatedWatra = new WatraCalculation()
            {
                Name = watraIn.Name,
                Description = watraIn.Description,
                WatraCalculationSection = new List<WatraCalculationSection>(),
                WatraCalculationMessage = new List<WatraCalculationMessage>(),
            };

            // Order pumps
            var sortedPumpSelections = pumpSelections.OrderBy(element => element.SortOrder).ToList();

            // Check some parameters
            if (minimalOutletPressure < this.RecommendedMinOutletPressure)
            {
                caluclatedWatra.WatraCalculationMessage.Add(
                new WatraCalculationMessage
                {
                    Severity = "info",
                    Message = $"Der geforderte Ausgangsdruck am Ende einer Wassertransportsektion ist kleiner als die empfohlenen Druck von mind. {this.RecommendedMinOutletPressure} bar.",
                });
            }

            if (watraIn.WatraRouteDistanceHeightElements?.Count <= 0)
            {
                caluclatedWatra.WatraCalculationMessage.Add(
                new WatraCalculationMessage
                {
                    Severity = "warning",
                    Message = $"Keine Distanz / Höhenelemente vorhanden.",
                });
            }

            if (pressureSafety < this.RecommendedSafetyPressure)
            {
                caluclatedWatra.WatraCalculationMessage.Add(
                new WatraCalculationMessage
                {
                    Severity = "warning",
                    Message = $"Der geforderte Sicherheitsdruck für die Berechnung ist kleiner als der empfohlenen Sicherheitsdurck von mind. {this.RecommendedSafetyPressure} bar.",
                });
            }

            caluclatedWatra.SafetyPressure = pressureSafety;

            // Calculate the actual flow rate
            var flowRate = flowRateIn;
            foreach (var currentPumpSelection in sortedPumpSelections)
            {
                flowRate = Math.Min(flowRate, currentPumpSelection.Pump.MaxFlowRateLitresPerMinute);
            }

            caluclatedWatra.ActualFlowRate = flowRate;

            if (flowRate != flowRateIn)
            {
                caluclatedWatra.WatraCalculationMessage.Add(
                new WatraCalculationMessage
                {
                    Severity = "warning",
                    Message = $"Die angeforderte Flussrate von {flowRateIn} l/min kann nicht von allen Pumpen erbracht werden, die tatsächliche Flussrate ist {flowRate} l/min",
                });
            }

            var counter = 0;
            foreach (var currentPumpSelection in sortedPumpSelections)
            {
                // it was defined that a pump can have either 1 or 2 hose lines, this is enforced by the validator
                var numberOfHoseLines = currentPumpSelection.NumberOfHoseLines;
                var pump = currentPumpSelection.Pump;

                // it was defined and will be inforced by the validator that
                //   - a pump can only have one hose type
                //   - at least one hose must be present
                //   - the hose fit to the pump and each other
                var connector = pump.Hose.HoseConnector;
                var hose = pump.Hose;
                var hoseDiameter = connector.Diameter;
                var hoseLength = hose.ElementLengthInMetres;

                // calculate start pressure after pump and make sure that it is smaller than max pump pressure
                double pressureStart = pump.MaxOutletPressureBar;
                if (pump.PumpPressureFlowModel != null)
                {
                    if (pump.PumpPressureFlowModel.Count > 0)
                    {
                        pressureStart = Math.Min(hydCalc.CalculatePumpPressue(this.ConvertToPolynom(pump.PumpPressureFlowModel), flowRate), pump.MaxOutletPressureBar);
                    }
                }

                // calculate hydrodynamic losses
                var pressureLossFrictionPerDistance = hydCalc.PressureLossFrictionPerDistance(hoseDiameter, flowRate);
                var pressureLossHeightPerHeight = hydCalc.PressureLossPerHeight();

                // initalize calculation of WatraElement
                double pressureEnd = pressureStart;
                double pressureLossFriction = 0.0;
                double pressureLossHeight = 0.0;
                double distance = 0.0;
                double heightDiff = 0.0;
                double maxHeightDiff = 0.0;

                while (distHeightCalcParam.CurrentPosition + distance < distHeightCalcParam.TotalLength && pressureEnd > (minimalOutletPressure + pressureSafety))
                {
                    distance += 1;
                    heightDiff = distHeightCalc.HeightDifferenceIn(distance, distHeightCalcParam);
                    maxHeightDiff = Math.Max(heightDiff, maxHeightDiff);

                    pressureLossFriction = distance * pressureLossFrictionPerDistance / Math.Pow(numberOfHoseLines, 2);
                    pressureLossHeight = heightDiff * pressureLossHeightPerHeight;
                    pressureEnd = pressureStart - pressureLossFriction - pressureLossHeight;
                }

                distHeightCalc.MoveBy(distance, distHeightCalcParam);

                var numberOfHoses = (int)Math.Ceiling(distance / hoseLength);
                var numberOfAddHoses = Math.Max(numberOfHoses - pump.NumberOfHoses, 0);

                if (numberOfAddHoses > 0)
                {
                    caluclatedWatra.WatraCalculationMessage.Add(
                        new WatraCalculationMessage
                        {
                            Severity = "warning",
                            Message = $"Sektion {counter} (Pumpenname: {pump.Name}) benötigt zusätzliche Schläuche ({numberOfAddHoses} Stück).",
                        });
                }

                // write results to WatraElement
                if (distance > 1)
                {
                    var watraCalulationElement = new WatraCalculationSection()
                    {
                        PressureAtSectionEnd = Math.Round(pressureEnd, 1),
                        PressureAtSectionStart = Math.Round(pressureStart, 1),
                        PressureLossFriction = Math.Round(pressureLossFriction, 1),
                        PressureLossHeight = Math.Round(pressureLossHeight, 1),
                        HeightPeak = Math.Round(maxHeightDiff, 1),
                        HeightDifference = Math.Round(heightDiff, 1),
                        SectionLength = Math.Round(distance, 1),
                        NameOfPump = pump.Name,
                        NumberOfHoseLines = numberOfHoseLines,
                        NameOfConnector = connector.Name,
                        DiameterConnector = connector.Diameter,
                        TotalHosesUsed = numberOfHoses,
                        ExternalHosesNeeded = numberOfAddHoses,
                    };
                    caluclatedWatra.WatraCalculationSection.Add(watraCalulationElement);

                    // Section counter
                    counter++;
                }
                else
                {
                    caluclatedWatra.WatraCalculationMessage.Add(
                        new WatraCalculationMessage
                        {
                            Severity = "info",
                            Message = $"Pumpe {pump.Name} ist für den Wassertransport ausgwählt, wird jedoch nicht benötigt.",
                        });
                }
            }

            // remaining distance
            caluclatedWatra.RemainingDistance = distHeightCalcParam.RemainingLength;
            if (caluclatedWatra.RemainingDistance > this.AcceptedRemainingDistance)
            {
                caluclatedWatra.WatraCalculationMessage.Add(
                new WatraCalculationMessage
                {
                    Severity = "error",
                    Message = $"Der Endpunkt des Wassertransport konnte mit den angegebnen Pumpen nicht erreicht werden. Distanz bis zum Ziel: {caluclatedWatra.RemainingDistance}m.",
                });
            }

            return caluclatedWatra;
        }

        private List<PolynomElement> ConvertToPolynom(List<PumpPressureFlowModelCoefficient> pumpPressureFlowCoefficients)
        {
            var polynomElements = new List<PolynomElement>();

            foreach (var pumpPressureFlowCoefficient in pumpPressureFlowCoefficients)
            {
                var polynomElement = new PolynomElement();
                polynomElement.Exponent = pumpPressureFlowCoefficient.Exponent;
                polynomElement.PolynomCoefficient = pumpPressureFlowCoefficient.Coefficient;
                polynomElements.Add(polynomElement);
            }

            return polynomElements;
        }
    }
}
