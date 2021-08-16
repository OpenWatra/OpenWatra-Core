// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api
{
    using System;
    using Watra.Tools;

    /// <summary>
    /// This calss keeps track of the current position (Unit: m) in the Watra and outputs the height differences (Unit: m) between a desired point and the current position.
    /// </summary>
    public class DistanceHeightCalculator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceHeightCalculator"/> class.
        /// Construct the calculator based on an existing List of WatraRouteElements.
        /// </summary>
        public DistanceHeightCalculator()
        {
        }

        /// <summary>
        /// Returns the height abvoe or below the start point of the Watra.
        /// </summary>
        /// <param name="position">Position in question, Unit: m</param>
        /// <returns>Height at position in question relative to the start height of the Watra, Unit: m</returns>
        public double HeightAt(double position, IDistanceHeightCalculationParameters param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "must not be null.");
            }

            if (!param.ValidPosition(position))
            {
                throw new ArgumentOutOfRangeException(nameof(position), "Target position is out of range.");
            }

            double sumHeight = 0.0;
            double sumLength = 0.0;
            bool foundWatraElement = false;

            var it = param.GetEnumerator();

            while (it.MoveNext() && !foundWatraElement)
            {
                if (sumLength + it.Current.Length <= position)
                {
                    sumLength += it.Current.Length;
                    sumHeight += it.Current.HeightDifference;
                }
                else
                {
                    if (it.Current.Length > 0.0)
                    {
                        sumHeight += MathTools.LinearInterpolation(position - sumLength, 0, it.Current.Length, 0.0, it.Current.HeightDifference);
                    }
                    else
                    {
                        sumHeight += it.Current.HeightDifference;
                    }

                    foundWatraElement = true;
                }
            }

            return sumHeight;
        }

        /// <summary>
        /// Calculates the height difference between the current position and a given distance to a point of interest.
        /// </summary>
        /// <param name="distance">Distance to point of interest, Unit: m</param>
        /// <returns>Height difference between current position and point of interest, Unit: m</returns>
        public double HeightDifferenceIn(double distance, IDistanceHeightCalculationParameters param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "must not be null.");
            }

            var targetPosition = param.CurrentPosition + distance;
            if (!param.ValidPosition(targetPosition))
            {
                throw new ArgumentOutOfRangeException(nameof(distance), "Target distance is out of range.");
            }

            return this.HeightAt(targetPosition, param) - param.CurrentHeight;
        }

        /// <summary>
        /// Move the current position to the specified target value, if it is valid.
        /// </summary>
        /// <param name="position">Target value, Unit: m</param>
        public void MoveTo(double position, IDistanceHeightCalculationParameters param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "must not be null.");
            }

            if (!param.ValidPosition(position))
            {
                throw new ArgumentOutOfRangeException(nameof(position), "Target position is out of range.");
            }

            param.SetCurrentPosition(position);
            param.SetCurrentHeight(this.HeightAt(position, param));
        }

        /// <summary>
        /// Moves the current position by a certain distance.
        /// </summary>
        /// <param name="distance">Distance to move by, Unit: m</param>
        public void MoveBy(double distance, IDistanceHeightCalculationParameters param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), "must not be null.");
            }

            var targetPosition = param.CurrentPosition + distance;
            if (!param.ValidPosition(targetPosition))
            {
                throw new ArgumentOutOfRangeException(nameof(distance), "Target distance is out of range.");
            }

            param.SetCurrentPosition(targetPosition);
            param.SetCurrentHeight(this.HeightAt(targetPosition, param));
        }
    }
}
