// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Web.ServerAccess
{
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;
    using Watra.Client.Web.ViewModel;

    /// <summary>
    /// Server access for Watra calculation.
    /// </summary>
    public interface IWatraCalculationServerAccess
    {
        /// <summary>
        /// Gets the Watra calculation of a watra with a given ID <paramref name="id"/>.
        /// </summary>
        Task<WatraCalculation> GetWatraCalculationByIdAsync(int id);

        Task<WatraCalculationViewModel> GetWatraCalculationByIdAsyncAsViewModel(int id);
    }
}
