// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System.Collections.Generic;

    /// <summary>
    /// Representation  of a pump within the database.
    /// </summary>
    public class DbPump : IDbEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="DbHoseConnector"/>s available with the pump.
        /// </summary>
        public DbHoseConnector HoseConnector { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="DbHose"/>s which come
        /// bundled with the pump.
        /// </summary>
        public DbHose Hose { get; set; }

        /// <summary>
        /// Gets or sets the number of hoses carried with the pump.
        /// </summary>
        public int NumberOfHoses { get; set; }

        /// <summary>
        /// Gets or sets flow Rate of the Pump in litres per minute.
        /// </summary>
        public double MaxFlowRateLitresPerMinute { get; set; }

        /// <summary>
        /// Gets or sets maximum pressure at the outlet in bar.
        /// </summary>
        public double MaxOutletPressureBar { get; set; }

        /// <summary>
        /// Gets or sets the pressure-flow-model of the pump.
        /// </summary>
        public List<DbPumpPressureFlowModelCoefficient> PumpPressureFlowModel { get; set; }

        /// <summary>
        /// Gets or sets a list of PumpSelections where this pump is used <see cref="DbPumpSelection"/>.
        /// One-To-Many relationship.
        /// All PumpSelections in which this pump is used are listed here.
        /// </summary>
        public List<DbPumpSelection> PumpSelection { get; set; }
    }
}
