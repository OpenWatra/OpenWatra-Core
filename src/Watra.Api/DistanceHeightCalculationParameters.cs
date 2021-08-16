// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using WaTra.Api.Server;

    /// <inheritdoc/>
    public class DistanceHeightCalculationParameters : IDistanceHeightCalculationParameters
    {
        private List<WatraRouteDistanceHeightElement> watraRouteElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceHeightCalculationParameters"/> class.
        /// </summary>
        public DistanceHeightCalculationParameters()
        {
            this.Init();
        }

        /// <inheritdoc/>
        public double TotalLength { get; private set; }

        /// <inheritdoc/>
        public double CurrentPosition { get; private set; }

        /// <inheritdoc/>
        public double CurrentHeight { get; private set; }

        /// <inheritdoc/>
        public double RemainingLength { get; private set; }

        /// <inheritdoc/>
        public bool ValidWatraLoaded { get; private set; }

        /// <inheritdoc/>
        public bool AddNewWatraElements(List<WatraRouteDistanceHeightElement> watraRouteElements)
        {
            var sortedElements = watraRouteElements.OrderBy(element => element.SortOrder).ToList();

            if (!this.ValidWatraRouteElementList(sortedElements))
            {
                this.Init();
            }
            else
            {
                this.watraRouteElements = sortedElements;
                this.ValidWatraLoaded = true;
                this.CalulateTotalLength();
                return true;
            }

            return this.ValidWatraLoaded;
        }

        /// <inheritdoc/>
        public bool SetCurrentPosition(double position)
        {
            if (!this.ValidPosition(position) && this.ValidWatraLoaded)
            {
                return false;
            }

            this.CurrentPosition = position;
            this.RemainingLength = this.TotalLength - this.CurrentPosition;
            return true;
        }

        /// <inheritdoc/>
        public bool SetCurrentHeight(double height)
        {
            if (!this.ValidWatraLoaded)
            {
                return false;
            }

            this.CurrentHeight = height;
            return true;
        }

        /// <inheritdoc/>
        public bool ValidPosition(double position)
        {
            return position >= 0.0 && position <= this.TotalLength;
        }

        /// <inheritdoc/>
        public List<WatraRouteDistanceHeightElement>.Enumerator GetEnumerator()
        {
            return this.watraRouteElements.GetEnumerator();
        }

        private void Init()
        {
            this.watraRouteElements = new List<WatraRouteDistanceHeightElement>();
            this.TotalLength = 0.0;
            this.CurrentPosition = 0.0;
            this.CurrentHeight = 0.0;
            this.RemainingLength = 0.0;
            this.ValidWatraLoaded = false;
        }

        private void CalulateTotalLength()
        {
            double totalLength = 0.0;
            for (int i = 0; i < this.watraRouteElements.Count; i++)
            {
                totalLength += this.watraRouteElements[i].Length;
            }

            this.TotalLength = totalLength;
        }

        private bool ValidWatraRouteElementList(List<WatraRouteDistanceHeightElement> watraRouteElements)
        {
            if (watraRouteElements == null)
            {
                return false;
            }

            foreach (var watraElement in watraRouteElements)
            {
                if (watraElement.Length < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
