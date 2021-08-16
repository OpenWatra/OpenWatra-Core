// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ViewModel
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// View Model representing a pump selection.
    /// </summary>
    [BindProperties]
    public class WatraCalculationMessageViewModel
    {
        /// <summary>
        /// Gets or sets serverity of the message, values are error, warning or info, all other keywords are debug messages.
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
