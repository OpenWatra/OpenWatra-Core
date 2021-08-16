// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;

    /// <summary>
    /// Server access for pumps.
    /// </summary>
    public interface IPumpServerAccess
    {
        /// <summary>
        /// Gets the pump with the ID <paramref name="id"/>.
        /// </summary>
        Task<Pump> GetPumpByIdAsync(int id);
    }
}
