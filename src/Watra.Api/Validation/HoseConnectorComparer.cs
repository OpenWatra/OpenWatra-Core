// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Validation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using WaTra.Api.Server;

    /// <summary>
    /// Compares two HoseConnector objects
    /// </summary>
    public class HoseConnectorComparer : EqualityComparer<HoseConnector>
    {
        /// <summary>
        /// Checks if two HoseConnector objects are equal.
        /// </summary>
        /// <param name="x">HoseConnector Object x</param>
        /// <param name="y">HoseConnector Object y</param>
        /// <returns>Wether x==y</returns>
        public override bool Equals(HoseConnector x, HoseConnector y)
        {
            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            return x.UniqueId.Equals(y.UniqueId);
        }

        /// <summary>
        /// Returns the HashCode of a HoseConnector object
        /// </summary>
        /// <param name="obj">HoseConnector Object</param>
        /// <returns>HashCode</returns>
        public override int GetHashCode([DisallowNull] HoseConnector obj)
        {
            return obj.UniqueId.GetHashCode();
        }
    }
}
