// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ViewModel;

    /// <summary>
    /// Services for
    /// </summary>
    public interface IWatraRouteServerAccess
    {
        /// <summary>
        /// Validates the <paramref name="watraRoute"/>.
        /// </summary>
        Task<ICollection<ValidationError>> ValidateWatraRouteAsync(WatraRoute watraRoute);

        /// <summary>
        /// Gets a task that loads all Watra routes from the database <see cref="WatraRoute"/>.
        /// </summary>
        Task GetAllWatraRouteAsync();

        /// <summary>
        /// Adds the <paramref name="watraRoute"/>.
        /// </summary>
        Task<WatraRoute> AddWatraRouteAsync(WatraRoute watraRoute);

        /// <summary>
        /// Returns the WatraRoute with the given <paramref name="id"/>.
        /// </summary>
        Task<WatraRoute> GetWatraRouteByIdAsync(int id);

        /// <summary>
        /// Updates the given <paramref name="watraRoute"/> on the server.
        /// </summary>
        Task<WatraRoute> UpdateWatraRouteAsync(WatraRoute watraRoute);

        /// <summary>
        /// Gets a list of <see cref="WatraRouteViewModel"/>, created using the available Watras in the database.
        /// </summary>
        Task<List<WatraRouteViewModel>> GetAllWatraRouteAsViewModelAsync();

        /// <summary>
        /// Gets a list of <see cref="WatraRouteViewModel"/>, created using the available Watras in the database.
        /// </summary>
        Task DeleteWatraRoute(int id);
    }
}
