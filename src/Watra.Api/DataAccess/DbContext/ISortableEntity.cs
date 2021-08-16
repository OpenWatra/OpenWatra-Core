// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System;

    /// <summary>
    /// Interface for entities supporting sorting.
    /// </summary>
    public interface ISortableEntity : IComparable<ISortableEntity>
    {
        /// <summary>
        /// Gets or sets the entitiy's sort order.
        /// </summary>
        int SortOrder { get; set; }
    }
}
