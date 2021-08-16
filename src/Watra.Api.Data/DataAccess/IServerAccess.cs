// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Data.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Watra.Api.Data.ApiClient;

    /// <summary>
    /// Generic interface to access the server API.
    /// </summary>
    /// <typeparam name="TModel">Type of the master data API object being accessed.</typeparam>
    public interface IServerAccess<TModel>
    {
        /// <summary>
        /// Gets all entities of the <typeparamref name="TModel"/>.
        /// </summary>
        public Task<ICollection<TModel>> GetAll();

        /// <summary>
        /// Creates or updates the <paramref name="model"/> on the database.
        /// </summary>
        public Task<TModel> UpdateAsync(TModel model);

        /// <summary>
        /// Gets all entities of the <typeparamref name="TModel"/>.
        /// </summary>
        public Task<ICollection<ValidationError>> Validate(TModel model);

        /// <summary>
        /// Deletes the <paramref name="model"/> in the database.
        /// </summary>
        public Task DeleteAsync(TModel model);
    }
}
