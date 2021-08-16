// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using System;

    /// <summary>
    /// Represents a hose connector in the database.
    /// </summary>
    public class DbHoseConnector : IDbEntity
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Diameter.
        /// </summary>
        public double Diameter { get; set; }

        /// <summary>
        /// Gets or sets a unique ID which can be used
        /// to determine whether hoses and pumps can be connected.
        /// If the Unique ID of the connector is the same for both
        /// they can be mechanically connected without using an adapter.
        /// </summary>
        public Guid UniqueId { get; set; }
    }
}
