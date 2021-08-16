// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// View Model representing a pump selection.
    /// </summary>
    [BindProperties]
    public class WatraCalculationViewModel
    {
        /// <summary>
        /// Gets or sets the Name of the Watra.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the Watra
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets safety pressure used to calculate this Watra, Unit: bar
        /// </summary>
        public double SafetyPressure { get; set; }

        /// <summary>
        /// Gets or sets flowrate used in this Watra section, Unit: l/min
        /// </summary>
        public double ActualFlowRate { get; set; }

        /// <summary>
        /// Gets or sets remaining distance to the end of the Watra, Unit: m
        /// </summary>
        public double RemainingDistance { get; set; }

        /// <summary>
        /// Gets or sets the list of Watra calculation section.
        /// </summary>
        public List<WatraCalculationSectionViewModel> WatraCalculationSection { get; set; }

        /// <summary>
        /// Gets or sets the list of Watra calculation messages.
        /// </summary>
        public List<WatraCalculationMessageViewModel> WatraCalculationMessage { get; set; }
    }
}
