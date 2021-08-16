// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a hose in the database.
    /// </summary>
    public class DbWatraRoute : IDbEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Watra is active, i.e. released for users to see.
        /// </summary>
        public bool IsActiveWatra { get; set; }

        /// <summary>
        /// Gets or sets the desired flow rate.
        /// </summary>
        public double FlowRate { get; set; }

        /// <summary>
        /// Gets or sets the minimal allowed outlet pressure at the end of the Watra, Unit: bar.
        /// </summary>
        public double MinimalOutletPressure { get; set; }

        /// <summary>
        /// Gets or sets pressure that is used as a safety factor during the calculation (recommended 2 bar), Unit: bar.
        /// </summary>
        public double SafetyPressure { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="DbPumpSelection"/>.
        /// Many-To-Many relationship.
        /// All PumpSelections which come packed with this watra are listed here.
        /// </summary>
        public List<DbPumpSelection> PumpSelections { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="DbWatraRouteDistanceHeightElement"/>.
        /// One-To-Many relationship.
        /// All WatraRouteDistanceHeightElements which come packed with this WatraRoute are listed here.
        /// </summary>
        public List<DbWatraRouteDistanceHeightElement> WatraRouteDistanceHeightElements { get; set; }
    }
}
