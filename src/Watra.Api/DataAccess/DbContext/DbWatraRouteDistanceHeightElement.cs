// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System;

    /// <summary>
    /// Represents a hose in the database.
    /// </summary>
    public class DbWatraRouteDistanceHeightElement : IDbEntity, ISortableEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the length of this part of the WatraRoute, Unit: m.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the height difference between start and end point of this part of the WatraRoute, Unit: m.
        /// </summary>
        public double HeightDifference { get; set; }

        /// <summary>
        /// Gets or sets a WatraRoute of <see cref="DbWatraRoute"/>.
        /// Many-To-One relationship.
        /// Each PumpSelections belongs to a WatraRoute.
        /// </summary>
        public DbWatraRoute WatraRoute { get; set; }

        /// <inheritdoc />
        public int SortOrder { get; set; }

        /// <inheritdoc />
        public int CompareTo(ISortableEntity other)
        {
            DbWatraRouteDistanceHeightElement otherDbDistHeightEl = other as DbWatraRouteDistanceHeightElement;
            if (otherDbDistHeightEl == null)
            {
                throw new ArgumentException($"Object is not a {nameof(DbWatraRouteDistanceHeightElement)}");
            }

            return this.SortOrder.CompareTo(otherDbDistHeightEl.SortOrder);
        }
    }
}
