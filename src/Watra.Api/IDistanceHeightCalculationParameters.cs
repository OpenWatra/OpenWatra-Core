// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api
{
    using System.Collections.Generic;
    using WaTra.Api.Server;

    /// <summary>
    /// Implementations of this interface keep track of the current state of a Watra, e.g. current position (Unit: m) in the Watra and height at current position (Unit: m).
    /// </summary>
    public interface IDistanceHeightCalculationParameters
    {
        /// <summary>
        /// Gets the total length of the Watra.
        /// </summary>
        public double TotalLength { get; }

        /// <summary>
        /// Gets the currently active position of the Watra calculation, Unit: m
        /// </summary>
        public double CurrentPosition { get; }

        /// <summary>
        /// Gets the height relative to the starting height of the Watra, Unit: m
        /// </summary>
        public double CurrentHeight { get; }

        /// <summary>
        /// Gets the remaining length to the end of the Watra, Unit: m
        /// </summary>
        public double RemainingLength { get; }

        /// <summary>
        /// Gets a value indicating whether a valid list of Watar elemetns is loaded.
        /// </summary>
        public bool ValidWatraLoaded { get; }

        /// <summary>
        /// Add a list of DbWatraRouteDistanceHeightElement from the database. Reinitalizes the class if called twice.
        /// </summary>
        /// <param name="watraRouteElements">List of DbWatraRouteDistanceHeightElement from the database.</param>
        /// <returns>True if the list was valid and accepted.</returns>
        public bool AddNewWatraElements(List<WatraRouteDistanceHeightElement> watraRouteElements);

        /// <summary>
        /// Sets the current position of the Watar.
        /// </summary>
        /// <param name="position">Current position, Unit: m</param>
        /// <returns>true if the parameter position was valid and the position was set.</returns>
        public bool SetCurrentPosition(double position);

        /// <summary>
        /// Sets the current height relative to the starting point of the Watar. Values smaller than zero are possible, i.e. current position is below the starting height.
        /// </summary>
        /// <param name="height">Current height relative to starting height of Watar, Unit: m</param>
        /// <returns>true if the parameter height was valid and the height was set.</returns>
        public bool SetCurrentHeight(double height);

        /// <summary>
        /// Checks if a position is valid, i.e. within the Watar.
        /// </summary>
        /// <param name="position">Position, Unit: m</param>
        /// <returns>true if the position is valid</returns>
        public bool ValidPosition(double position);

        /// <summary>
        /// Returns the enumerator for the list of DbWatraRouteDistanceHeightElement.
        /// </summary>
        /// <returns>Enumerator of the list.</returns>
        public List<WatraRouteDistanceHeightElement>.Enumerator GetEnumerator();
    }
}
