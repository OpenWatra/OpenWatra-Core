// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    /// <summary>
    /// Interface for hydrodynamic calculations with water at temperatures from 5..25°C
    /// </summary>
    public interface IHydrodynamicCalculator
    {
        /// <summary>
        /// Gets viscosity used for hydrodynamic calculations, Unit: Pa*s
        /// </summary>
        public double Viscosity { get; }

        /// <summary>
        /// Gets water temperature, Unit: °C
        /// </summary>
        public double WaterTemperature { get; }

        /// <summary>
        /// Changes the water temperature.
        /// </summary>
        /// <param name="waterTemperature">Water temperature, Unit: °C, range: 5..25°C, standard value: 10°C</param>
        public void SetWaterTemperature(double waterTemperature);

        /// <summary>
        /// Calculates the Reynolds number for a pipe with a round cross-section
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="volumeFlowSpeed">Volume flow speed inside pipe, Unit: m/s</param>
        /// <returns>Reynolds number of flow inside pipe with round cross-section, Unit:1</returns>
        public double ReynoldsNumber(double diameterPipe, double volumeFlowSpeed);

        /// <summary>
        /// Returns the volume flow speed, in a round pipe with diamter diameterPipe and a flow rate of flowRate.
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="flowRate">Flow rate through pipe, Unit: l/min</param>
        /// <returns>Volume flow speed in round pipe, Unit: m/s</returns>
        public double VolumeFlowSpeed(double diameterPipe, double flowRate);

        /// <summary>
        /// Calculates the pressure loss due to hydrodynamic friction of a turbulent flow in pipes with a round cross-section.
        /// </summary>
        /// <param name="diameterPipe">Diameter of pipe, Unit: mm</param>
        /// <param name="flowRate">Flow rate through pipe, Unit: l/min</param>
        /// <returns>Pressure loss due to friction per length of pipe, Unit: bar/m</returns>
        public double PressureLossFrictionPerDistance(double diameterPipe, double flowRate);

        /// <summary>
        /// Returns the pressure loss relative to height difference.
        /// </summary>
        /// <returns>Returns the pressure loss relative to height difference (height difference in meter), Unit: bar/m</returns>
        public double PressureLossPerHeight();

        /// <summary>
        /// Returns the friction coefficient of a turbulent flow in a pipe, for pipes with hydraulically smooth surfaces.
        /// </summary>
        /// <param name="reynoldsNumber">Reynolds number of flow inside the pipe, Unit: 1</param>
        /// <returns>Friction coefficient of turbulent flow in pipe, Unit: 1</returns>
        public double PipeFrictionCoefficient(double reynoldsNumber);
    }
}
