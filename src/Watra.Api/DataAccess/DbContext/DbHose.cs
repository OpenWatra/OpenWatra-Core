// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    /// <summary>
    /// Represents a hose in the database.
    /// </summary>
    public class DbHose : IDbEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the element length in meters.
        /// </summary>
        public double ElementLengthInMetres { get; set; }

        /// <summary>
        /// Gets or sets the hose connector piece.
        /// </summary>
        public DbHoseConnector HoseConnector { get; set; }
    }
}
