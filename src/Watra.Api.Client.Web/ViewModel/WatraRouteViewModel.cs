// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// View Model representing a pump selection.
    /// </summary>
    [BindProperties]
    public class WatraRouteViewModel
    {
        /// <summary>
        /// Gets or sets the ID of the Watra.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Watra.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the Watra
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Watra is active, i.e. released for users to see.
        /// </summary>
        public bool IsActiveWatra { get; set; }
    }
}
