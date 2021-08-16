// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using Microsoft.AspNetCore.Mvc;
    using Watra.Api.Data.ApiClient;

    /// <summary>
    /// View Model representing a pump selection.
    /// </summary>
    [BindProperties]
    public class PumpSelectionViewModel
    {
        /// <summary>
        /// Gets or sets the sort order of this pump selection.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the pump represented by this view model.
        /// </summary>
        public Pump Pump { get; set; }

        /// <summary>
        /// Gets or sets the ID of the pump represented by the selection.
        /// </summary>
        public int IdPump { get; set; }

        /// <summary>
        /// Gets the Name of the pump.
        /// </summary>
        public string Name => this.Pump?.Name;

        /// <summary>
        /// Gets or sets a value indicating whether the pump is selected for the WaTra.
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
