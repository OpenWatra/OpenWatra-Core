// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    /// <summary>
    /// Interface from which any database entity must inherit.
    /// </summary>
    public interface IDbEntity
    {
        /// <summary>
        /// Gets or sets the entities ID.
        /// </summary>
        public int Id { get; set; }
    }
}
