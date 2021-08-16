// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Used for hydrodynamic calculations with water at temperatures from 5..25°C
    /// Formelsammlung auf https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php.
    /// Annahmen:
    ///   - Die Strömung ist turbulent und reibungsbehaftet.
    ///   - Die Druckverluste können mit der verlustbehafeten Bernoulli Energiegleichung berechnet werden
    ///   - Die verwendeten Schläuche sind hydraulisch glatt und die Reibungsverluste lassen sich mit der
    ///     Rohrreibungszahl nach Prandtl und K'arm'an approximieren.
    ///   - Der Querschnitt eines Schlauches ist immer rund.
    ///   - Ein Schlauch ist immer ganz gefüllt.
    ///   - Die Temperaturabhängigkeit der Viskosität wird berücksichtigt, ansonsten gilt das Fluid (Wasser)
    ///     als inkrompessibel und stationär.
    /// </summary>
    public class HydrodynamicCalculator : IHydrodynamicCalculator
    {
        // Gravitational acceleration, Unit: m/s^2
        private const double GravitationalAcceleration = 9.80665;

        // Density of water, Unit: kg/m^3
        private const double WaterDensity = 997.0;

        // Temperature boundaries, Unit: °C
        private const double LowerTemperatureBoundary = 0;
        private const double UpperTemperatureBoundary = 50;

        // Viscosity of water at <Temperature, Viscosity>, Units: <°C, Pa*s>
        private SortedDictionary<double, double> viscosityTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="HydrodynamicCalculator"/> class.
        /// </summary>
        /// <param name="waterTemperature">Set water temperature, Unit: °C, standard value: 10°C</param>
        public HydrodynamicCalculator(double waterTemperatur = 10.0)
        {
            // initialze viskosity table
            this.viscosityTable = new SortedDictionary<double, double>();
            this.InitViscosityTable();

            // set Water temperatur, standard value is 10°C, allowed values are 5..25°C
            this.SetWaterTemperature(waterTemperatur);
        }

        /// <summary>
        /// Gets viscosity used for hydrodynamic calculations, Unit: Pa*s
        /// </summary>
        public double Viscosity { get; private set; }

        /// <summary>
        /// Gets water temperature, Unit: °C
        /// </summary>
        public double WaterTemperature { get; private set; }

        /// <summary>
        /// Changes the water temperature.
        /// </summary>
        /// <param name="waterTemperature">Water temperature, Unit: °C, range: 5..25°C, standard value: 10°C</param>
        public void SetWaterTemperature(double waterTemperature)
        {
            if (waterTemperature < LowerTemperatureBoundary || waterTemperature > UpperTemperatureBoundary)
            {
                throw new ArgumentOutOfRangeException("waterTemperature", $"must be {LowerTemperatureBoundary}..{UpperTemperatureBoundary}°C!");
            }

            this.WaterTemperature = waterTemperature;

            // Update viscosity according to temperatur
            this.UpdateViscosity();
        }

        /// <summary>
        /// Calculates the Reynolds number for a pipe with a round cross-section
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="volumeFlowSpeed">Volume flow speed inside pipe, Unit: m/s</param>
        /// <returns>Reynolds number of flow inside pipe with round cross-section, Unit:1</returns>
        public double ReynoldsNumber(double diameterPipe, double volumeFlowSpeed)
        {
            double diameterPipeInM = diameterPipe * Math.Pow(10, -3); // calculate diameterPipe in m
            return diameterPipeInM * volumeFlowSpeed * WaterDensity / this.Viscosity;
        }

        /// <summary>
        /// Calculates the volume flow from a given flow rate.
        /// </summary>
        /// <param name="flowRate">Flow rate, Unit: l/min</param>
        /// <returns>Volume flow, Unit: m^3/s</returns>
        public double VolumeFlow(double flowRate)
        {
            double flowRateInm3PerS = flowRate / 60.0 * Math.Pow(10, -3); // calculate flowRate in m^3/s

            return flowRateInm3PerS;
        }

        /// <summary>
        /// Returns the volume flow speed, in a round pipe with diamter diameterPipe and a flow rate of flowRate.
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="flowRate">Flow rate through pipe, Unit: l/min</param>
        /// <returns>Volume flow speed in round pipe, Unit: m/s</returns>
        public double VolumeFlowSpeed(double diameterPipe, double flowRate)
        {
            double flowRateInm3PerS = this.VolumeFlow(flowRate); // calculate flowRate in m^3/s
            double diameterPipeInM = diameterPipe * Math.Pow(10, -3); // calculate diameterPipe in m

            double radiusInM = diameterPipeInM / 2.0;

            if (radiusInM <= 0.0)
            {
                return double.PositiveInfinity;
            }

            return flowRateInm3PerS / (Math.Pow(radiusInM, 2) * Math.PI);
        }

        /// <summary>
        /// Calculates the pressure loss due to hydrodynamic friction of a turbulent flow in pipes with a round cross-section.
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="flowRate">Flow rate through pipe, Unit: l/min</param>
        /// <returns>Pressure loss due to friction per length of pipe, Unit: bar/m</returns>
        public double PressureLossFrictionPerDistance(double diameterPipe, double flowRate)
        {
            double volumeFlowSpeed = this.VolumeFlowSpeed(diameterPipe, flowRate);
            double reynoldsNumber = this.ReynoldsNumber(diameterPipe, volumeFlowSpeed);
            double frictionCoefficient = this.PipeFrictionCoefficient(reynoldsNumber);
            double diameterPipeInM = diameterPipe * Math.Pow(10, -3);
            double flowRateInm3PerS = this.VolumeFlow(flowRate);

            return frictionCoefficient * WaterDensity * 8 / (Math.Pow(diameterPipeInM, 5) * Math.Pow(Math.PI, 2)) * Math.Pow(flowRateInm3PerS, 2) * Math.Pow(10, -5);
        }

        /// <summary>
        /// Returns the pressure loss relative to height difference.
        /// </summary>
        /// <returns>Returns the pressure loss relative to height difference (height difference in meter), Unit: bar/m</returns>
        public double PressureLossPerHeight()
        {
            return WaterDensity * GravitationalAcceleration * Math.Pow(10, -5);
        }

        /// <summary>
        /// Returns the friction coefficient of a turbulent flow in a pipe, for pipes with hydraulically smooth surfaces.
        /// </summary>
        /// <param name="reynoldsNumber">Reynolds number of flow inside the pipe, Unit: 1</param>
        /// <returns>Friction coefficient of turbulent flow in pipe, Unit: 1</returns>
        public double PipeFrictionCoefficient(double reynoldsNumber)
        {
            double frictionCoefficient = 0.2; // inital value
            for (int i = 0; i < 25; i++)
            {
                var divisor = 2.0 * Math.Log10(reynoldsNumber * Math.Sqrt(frictionCoefficient) / 2.51);
                frictionCoefficient = Math.Pow(1 / divisor, 2);
            }

            return frictionCoefficient;
        }

        /// <summary>
        ///  Calculates the output pressure of the pump at a given flow rate x, such that y=A[0]*x^0+A[1]*x^1+..., where y is the output pressure of the pump in bar and x is the flowrate in l/min.
        /// </summary>
        /// <param name="polynomial">List with polynom elements, such that one element represents coefficient, e.g. c0 and the corresponding exponent of
        /// y= c0 + c1 * x^1 + c2 * x^2 +..., where y is the output pressure of the pump in bar and x is the flowrate of the pump in l/min.</param>
        /// <param name="flowRate">Flow rate of the pump, Unit: l/min</param>
        /// <returns>Output pressure of the pump, Unit: bar</returns>
        public double CalculatePumpPressue(List<PolynomElement> polynomial, double flowRate)
        {
            double sum = 0.0;
            if (polynomial != null)
            {
                for (int i = 0; i < polynomial.Count; i++)
                {
                    sum = sum + (polynomial[i].PolynomCoefficient * Math.Pow(flowRate, polynomial[i].Exponent));
                }
            }

            return sum;
        }

        private void UpdateViscosity()
        {
            KeyValuePair<double, double>? upperPair = null;
            KeyValuePair<double, double>? lowerPair = null;
            var enumerator = this.viscosityTable.GetEnumerator();

            foreach (var it in this.viscosityTable)
            {
                if (this.WaterTemperature < it.Key)
                {
                    upperPair = it;
                    break;
                }
                else
                {
                    lowerPair = it;
                }
            }

            // interolation not possible because WaterTemperature is below smallest key of dictonary
            if (!lowerPair.HasValue)
            {
                if (!upperPair.HasValue)
                {
                    // dictonary is empty
                    throw new ArgumentOutOfRangeException("viscosityTable is empty");
                }

                this.Viscosity = upperPair.Value.Value;
                return;
            }

            // interolation not possible because WaterTemperature is above greatest key of dictonary
            if (!upperPair.HasValue)
            {
                if (!lowerPair.HasValue)
                {
                    // dictonary is empty
                    throw new ArgumentOutOfRangeException("viscosityTable is empty");
                }

                this.Viscosity = lowerPair.Value.Value;
                return;
            }

            // linear interpolation between lower and upper pair
            this.Viscosity = MathTools.LinearInterpolation(this.WaterTemperature, lowerPair.Value.Key, upperPair.Value.Key, lowerPair.Value.Value, upperPair.Value.Value);
        }

        private void InitViscosityTable()
        {
            // Use standard IComparable<T> Interface for double

            // Add values
            this.viscosityTable.Add(5.0, 0.00152);
            this.viscosityTable.Add(10.0, 0.001297);
            this.viscosityTable.Add(20.0, 0.001);
            this.viscosityTable.Add(25.0, 0.000891);
        }
    }
}
