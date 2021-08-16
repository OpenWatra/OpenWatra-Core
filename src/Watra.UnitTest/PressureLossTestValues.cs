// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools.Tests
{
    /// <summary>
    /// Holds test values for pressure loss due two friction unit test.
    /// </summary>
    internal class PressureLossTestValues
    {
        /// <summary>
        /// Gets or sets pipe diameter, Unit: mm
        /// </summary>
        public double PipeDiameter { get; set; }

        /// <summary>
        /// Gets or sets flow rate, Unit: l/min
        /// </summary>
        public double FlowRate { get; set; }

        /// <summary>
        /// Gets or sets length of pipe, Unit: m
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets expected pressure loss, Unit: bar
        /// </summary>
        public double PressureLoss { get; set; }
    }
}
