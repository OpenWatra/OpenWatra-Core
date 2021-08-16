// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System;

    /// <summary>
    /// Represents a hose in the database.
    /// </summary>
    public class DbPumpSelection : IDbEntity, ISortableEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the number of hose lines used for this pump.
        /// </summary>
        public int NumberOfHoseLines { get; set; }

        /// <summary>
        /// Gets or sets the Foreign key - ID of the <see cref="Pump"/>.
        /// </summary>
        public int PumpId { get; set; }

        /// <summary>
        /// Gets or sets the pump.
        /// </summary>
        public DbPump Pump { get; set; }

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
            DbPumpSelection otherDbPumpSelection = other as DbPumpSelection;
            if (otherDbPumpSelection == null)
            {
                throw new ArgumentException($"Object is not a {nameof(DbPumpSelection)}");
            }

            return this.SortOrder.CompareTo(otherDbPumpSelection.SortOrder);
        }
    }
}
