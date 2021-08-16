// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Watra.Api;
    using WaTra.Api.Server;

    /// <summary>
    /// Test class for the DistanceHeightCalculator class.
    /// </summary>
    [TestClass]
    public class DistanceHeightCalculatorTest
    {
        /// <summary>
        /// Test calculation of total route length.
        /// </summary>
        [TestMethod]
        public void TestTotalLength()
        {
            List<WatraRouteDistanceHeightElement> watraElements = this.CreateWatraElementsRandom(10);

            double totalLengthExpected = 0.0;
            foreach (var element in watraElements)
            {
                totalLengthExpected += element.Length;
            }

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);

            Assert.AreEqual(distHeightParam.TotalLength, totalLengthExpected, 0.1);
        }

        /// <summary>
        /// Test check for valid position.
        /// </summary>
        [TestMethod]
        public void TestValidPosition()
        {
            List<WatraRouteDistanceHeightElement> watraElements = this.CreateWatraElementsRandom(10);

            double totalLengthExpected = 0.0;
            foreach (var element in watraElements)
            {
                totalLengthExpected += element.Length;
            }

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);

            Assert.AreEqual(distHeightParam.ValidPosition(-100), false);
            Assert.AreEqual(distHeightParam.ValidPosition(totalLengthExpected / 2.0), true);
            Assert.AreEqual(distHeightParam.ValidPosition(totalLengthExpected), true);
            Assert.AreEqual(distHeightParam.ValidPosition(totalLengthExpected + 100), false);
        }

        /// <summary>
        /// Test implementation of MoveTo.
        /// </summary>
        [TestMethod]
        public void TestMoveTo()
        {
            // create list of watraElements of length 500m with heigt difference of 50m per element
            List<WatraRouteDistanceHeightElement> watraElements = new List<WatraRouteDistanceHeightElement>();
            for (int i = 0; i < 10; i++)
            {
                var watraElement = new WatraRouteDistanceHeightElement();
                watraElement.Length = 500.0;
                watraElement.HeightDifference = 50.0;
                watraElements.Add(watraElement);
            }

            // initalize calculator
            double expectedPosition = 0.0;
            double expectedHeight = 0.0;

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);
            var distHeightCalc = new DistanceHeightCalculator();

            // Move to some positions
            for (int i = 0; i < 10; i++)
            {
                expectedPosition = 300.0 * i;
                expectedHeight = 0.1 * expectedPosition;

                distHeightCalc.MoveTo(expectedPosition, distHeightParam);

                Assert.AreEqual(distHeightParam.CurrentPosition, expectedPosition);
                Assert.AreEqual(distHeightParam.CurrentHeight, expectedHeight);
            }

            // Special case: try to move outside total length
            var lastPosition = distHeightParam.CurrentPosition;
            var lastHeight = distHeightParam.CurrentHeight;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => distHeightCalc.MoveTo(11 * 500, distHeightParam));
            Assert.AreEqual(distHeightParam.CurrentPosition, lastPosition);
            Assert.AreEqual(distHeightParam.CurrentHeight, lastHeight);

            // Special case: add a step, i.e. element with 0m length
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 0,
                    HeightDifference = 200,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 1000,
                    HeightDifference = 1000,
                });

            distHeightParam.AddNewWatraElements(watraElements);

            expectedPosition = 10 * 500.0;
            expectedHeight = (10 * 50.0) + 200;

            distHeightCalc.MoveTo(expectedPosition, distHeightParam);

            Assert.AreEqual(distHeightParam.CurrentPosition, expectedPosition);
            Assert.AreEqual(distHeightParam.CurrentHeight, expectedHeight);

            // Special case: move to end
            expectedPosition = (10 * 500.0) + 1000;
            expectedHeight = (10 * 50.0) + 200 + 1000;

            distHeightCalc.MoveTo(expectedPosition, distHeightParam);

            Assert.AreEqual(distHeightParam.CurrentPosition, expectedPosition);
            Assert.AreEqual(distHeightParam.CurrentHeight, expectedHeight);
        }

        /// <summary>
        /// Test implementation of MoveBy.
        /// </summary>
        [TestMethod]
        public void TestMoveBy()
        {
            // create list of watraElements of length 500m with heigt difference of 50m per element
            List<WatraRouteDistanceHeightElement> watraElements = new List<WatraRouteDistanceHeightElement>();
            for (int i = 0; i < 10; i++)
            {
                var watraElement = new WatraRouteDistanceHeightElement();
                watraElement.Length = 500.0;
                watraElement.HeightDifference = 50.0;
                watraElements.Add(watraElement);
            }

            // initalize calculator
            double expectedPosition = 0.0;
            double expectedHeight = 0.0;

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);
            var distHeightCalc = new DistanceHeightCalculator();

            // Move to some positions
            for (int i = 0; i < 10; i++)
            {
                expectedPosition += 300.0;
                expectedHeight = 0.1 * expectedPosition;

                distHeightCalc.MoveBy(300, distHeightParam);

                Assert.AreEqual(distHeightParam.CurrentPosition, expectedPosition);
                Assert.AreEqual(distHeightParam.CurrentHeight, expectedHeight);
            }

            // Special case: try to move outside total length
            var lastPosition = distHeightParam.CurrentPosition;
            var lastHeight = distHeightParam.CurrentHeight;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => distHeightCalc.MoveBy(11 * 500, distHeightParam));
            Assert.AreEqual(distHeightParam.CurrentPosition, lastPosition);
            Assert.AreEqual(distHeightParam.CurrentHeight, lastHeight);
        }

        /// <summary>
        /// Test implementation of HeightAt.
        /// </summary>
        [TestMethod]
        public void TestHeightAt()
        {
            // create list of watraElements of length 500m with heigt difference of 50m per element
            List<WatraRouteDistanceHeightElement> watraElements = new List<WatraRouteDistanceHeightElement>();

            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 500,
                    HeightDifference = 100,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 0,
                    HeightDifference = 10,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 300,
                    HeightDifference = -60,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 700,
                    HeightDifference = 30,
                });

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);
            var distHeightCalc = new DistanceHeightCalculator();

            double[] position = { 100.0, 499.0, 517.0, 603.0, 1199.0 };
            double[] expectedHeight = { 20.0, 99.8, 106.6, 89.4, 67.1 };

            if (position.Length == expectedHeight.Length)
            {
                for (int i = 0; i < expectedHeight.Length; i++)
                {
                    Assert.AreEqual(distHeightCalc.HeightAt(position[i], distHeightParam), expectedHeight[i]);
                }
            }
            else
            {
                throw new InvalidOperationException("position and expectedHeight must be of the same length");
            }
        }

        /// <summary>
        /// Test implementation of HeightDifferenceIn.
        /// </summary>
        [TestMethod]
        public void TestHeightDifferenceIn()
        {
            // create list of watraElements of length 500m with heigt difference of 50m per element
            List<WatraRouteDistanceHeightElement> watraElements = new List<WatraRouteDistanceHeightElement>();

            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 500,
                    HeightDifference = 100,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 500,
                    HeightDifference = -50,
                });
            watraElements.Add(
                new WatraRouteDistanceHeightElement()
                {
                    Length = 500,
                    HeightDifference = 100,
                });

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);
            var distHeightCalc = new DistanceHeightCalculator();

            distHeightCalc.MoveTo(500, distHeightParam);

            double[] distance = { -500, 500, 1000 };
            double[] expectedHeight = { -100, -50, 50 };

            if (distance.Length == expectedHeight.Length)
            {
                for (int i = 0; i < expectedHeight.Length; i++)
                {
                    Assert.AreEqual(distHeightCalc.HeightDifferenceIn(distance[i], distHeightParam), expectedHeight[i]);
                }
            }
            else
            {
                throw new InvalidOperationException("position and expectedHeight must be of the same length");
            }
        }

        /// <summary>
        /// Test implementation of RemainingLength.
        /// </summary>
        [TestMethod]
        public void TestRemainingLength()
        {
            var random = new Random();

            // create list of watraElements of length 500m with heigt difference of 50m per element
            List<WatraRouteDistanceHeightElement> watraElements = this.CreateWatraElementsRandom(3);

            double totalLengthExpected = 0.0;
            foreach (var element in watraElements)
            {
                totalLengthExpected += element.Length;
            }

            var distHeightParam = new DistanceHeightCalculationParameters();
            distHeightParam.AddNewWatraElements(watraElements);
            var distHeightCalc = new DistanceHeightCalculator();

            var position = totalLengthExpected * random.NextDouble();

            distHeightCalc.MoveTo(position, distHeightParam);

            Assert.AreEqual(distHeightParam.RemainingLength, totalLengthExpected - position);
        }

        private List<WatraRouteDistanceHeightElement> CreateWatraElementsRandom(int count)
        {
            Random random = new Random();
            List<WatraRouteDistanceHeightElement> watraElements = new List<WatraRouteDistanceHeightElement>();

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var watraElement = new WatraRouteDistanceHeightElement();
                    watraElement.Length = (double)random.Next(0, 2000);
                    watraElement.HeightDifference = (double)random.Next(-200, 200);
                    watraElements.Add(watraElement);
                }
            }

            return watraElements;
        }
    }
}
