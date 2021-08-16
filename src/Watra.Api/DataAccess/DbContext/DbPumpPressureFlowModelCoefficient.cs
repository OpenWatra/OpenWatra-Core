// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    /// <summary>
    /// Represents a hose in the database.
    /// </summary>
    public class DbPumpPressureFlowModelCoefficient : IDbEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets polynomal coefficient.
        /// </summary>
        public double Coefficient { get; set; }

        /// <summary>
        /// Gets or sets exponent of polynomal.
        /// </summary>
        public int Exponent { get; set; }

        /// <summary>
        /// Gets or sets the corresponding PumpPressureFlowModel.
        /// </summary>
        public DbPump Pump { get; set; }
    }
}
