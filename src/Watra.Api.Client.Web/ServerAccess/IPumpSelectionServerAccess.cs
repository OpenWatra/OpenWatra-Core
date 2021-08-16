// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Client.Web.ViewModel;

    /// <summary>
    /// Access class for <see cref="PumpSelection"/> and view models handling it.
    /// </summary>
    public interface IPumpSelectionServerAccess
    {
        /// <summary>
        /// Gets a list of <see cref="PumpSelectionViewModel"/>, created using the available pumps in the database.
        /// </summary>
        Task<List<PumpSelectionViewModel>> GetPossiblePumpSelectionsAsViewModelsAsync();
    }
}
