// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// View Model representing a pump selection.
    /// </summary>
    [BindProperties]
    public class WatraCalculationSectionViewModel
    {
        /// <summary>
        /// Gets or sets length of this section of the Watra, Unit: m
        /// </summary>
        public double SectionLength { get; set; }

        /// <summary>
        /// Gets or sets height difference between start and end point of this section of the Watra, Unit: m
        /// </summary>
        public double HeightDifference { get; set; }

        /// <summary>
        /// Gets or sets heighest point above start point of this section, Unit: m
        /// </summary>
        public double HeightPeak { get; set; }

        /// <summary>
        /// Gets or sets pressure at section start, Unit: m
        /// </summary>
        public double PressureAtSectionStart { get; set; }

        /// <summary>
        /// Gets or sets pressure loss in this section due to height, Unit: bar
        /// </summary>
        public double PressureLossHeight { get; set; }

        /// <summary>
        /// Gets or sets pressure loss in this section due to hydrodynamic friction in all hose lines, Unit: bar
        /// </summary>
        public double PressureLossFriction { get; set; }

        /// <summary>
        /// Gets or sets pressure at end of hose line of this section, Unit: bar
        /// </summary>
        public double PressureAtSectionEnd { get; set; }

        /// <summary>
        /// Gets or sets the NameOfPump.
        /// </summary>
        public string NameOfPump { get; set; }

        /// <summary>
        /// Gets or sets the name of the used connector.
        /// </summary>
        public string NameOfConnector { get; set; }

        /// <summary>
        /// Gets or sets the DiameterConnector.
        /// </summary>
        public double DiameterConnector { get; set; }

        /// <summary>
        /// Gets or sets the number of parallel hose lines used in this section.
        /// </summary>
        public int NumberOfHoseLines { get; set; }

        /// <summary>
        /// Gets or sets the number of total number of hoses used.
        /// </summary>
        public int TotalHosesUsed { get; set; }

        /// <summary>
        /// Gets or sets the number of hoses used that are not stored on the pump.
        /// </summary>
        public int ExternalHosesNeeded { get; set; }
    }
}
