// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Watra.Tools;

    /// <summary>
    /// Test class for the HydrodynamicCalculator class.
    /// </summary>
    [TestClass]
    public class HydrodynamicCalculatorTest
    {
        /// <summary>
        /// Tests the flow speed calculation according to pre calculated values.
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// were calculated by hand to recieve the expected values for the test.
        /// </summary>
        [TestMethod]
        public void TestVolumeFlowSpeed()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();

            double[] diameterPipe = { 150, 150, 110, 110, 75, 75, 55, 55 }; // mm
            double[] flowRate = { 4500, 2100, 2800, 1000, 1000, 500, 500, 300 }; // l/min
            double[] expected = { 4.24, 1.98, 4.91, 1.75, 3.77, 1.89, 3.51, 2.10 }; // m/s

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(hydCalc.VolumeFlowSpeed(diameterPipe[i], flowRate[i]), expected[i], 0.1);
            }

            // check zero parameters
            Assert.AreEqual(hydCalc.VolumeFlowSpeed(0, 0), double.PositiveInfinity);
        }

        /// <summary>
        /// Tests the calculation of the reynolds number according to pre calculated values.
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// were calculated by hand to recieve the expected values for the test.
        /// </summary>
        [TestMethod]
        public void TestReynoldsNumber()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();

            double[] diameterPipe = { 150, 150, 150, 110, 110, 75, 55, 55 }; // mm
            double[] volumeFlowSpeed = { 4.24413, 1.98059, 2.64079, 1.75377, 1.40302, 1.88628, 3.50755, 2.10453 }; // m/s
            double[] expected = { 489367.7, 228371.6, 304495.5, 148293.2, 118634.6, 108748.4, 148293.2, 88975.9 }; // 1

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(hydCalc.ReynoldsNumber(diameterPipe[i], volumeFlowSpeed[i]), expected[i], 1);
            }

            // check zero parameters
            Assert.AreEqual(hydCalc.ReynoldsNumber(0, 0), 0.0);
        }

        /// <summary>
        /// Tests the set water temperature and the resulting change in the visosity.
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// were calculated by hand to recieve the expected values for the test.
        /// </summary>
        [TestMethod]
        public void TestSetWaterTemperature()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();

            // Viscosity at 10°C
            Assert.AreEqual(hydCalc.Viscosity, 0.001297, 0.00001);

            // Viscosity below 5°C equals the one at 5°C
            hydCalc.SetWaterTemperature(1.0);
            Assert.AreEqual(hydCalc.Viscosity, 0.00152, 0.00001);

            // Viscosity below 25°C equals the one at 25°C
            hydCalc.SetWaterTemperature(45.0);
            Assert.AreEqual(hydCalc.Viscosity, 0.000891, 0.00001);

            // Linear interpolation works
            hydCalc.SetWaterTemperature(18.0);
            Assert.AreEqual(hydCalc.Viscosity, 0.0010594, 0.00001);

            // Check if Exceptions occure
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hydCalc.SetWaterTemperature(55.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hydCalc.SetWaterTemperature(-1));
        }

        /// <summary>
        /// Tests the pressure flow model of a pump.
        /// Values for this test were optained from Feuerwehr Zug,
        /// Excel Druckberechnung_Vorlage.xlsx, with values from manual of the pump.
        /// </summary>
        [TestMethod]
        public void TestCalculatePumpPressue()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();
            double flowRate = 2000; // l/min

            // Polynom for Hydro-Sub outlet pressure by given flow rate (source Feuerwehr Zug, Excel Druckberechnung_Vorlage.xlsx)
            List<PolynomElement> polynom = new List<PolynomElement>()
            {
                new PolynomElement()
                {
                    PolynomCoefficient = -7.8994 * Math.Pow(10, -7),
                    Exponent = 2,
                },
                new PolynomElement()
                {
                    PolynomCoefficient = 0.0023,
                    Exponent = 1,
                },
                new PolynomElement()
                {
                    PolynomCoefficient = 12.3762,
                    Exponent = 0,
                },
            };

            double expected = 13.9; // bar
            double result = hydCalc.CalculatePumpPressue(polynom, flowRate);

            Assert.AreEqual(expected, result, 0.1);
        }

        /// <summary>
        /// Tests the pipe friction coefficient.
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// were calculated by hand to recieve the expected values for the test.
        /// </summary>
        [TestMethod]
        public void PipeFrictionCoefficientTest()
        {
            var hydCalc = new HydrodynamicCalculator();
            double[] reynoldsNumber = { 148293.2439, 29658.64877 }; // 1
            double[] expectedCoefficient = { 0.0166, 0.0235 }; // 1

            for (int i = 0; i < reynoldsNumber.Length; i++)
            {
                var calculatedCoefficient = hydCalc.PipeFrictionCoefficient(reynoldsNumber[i]);
                Assert.AreEqual(expectedCoefficient[i], calculatedCoefficient, 0.0001);
            }
        }

        /// <summary>
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// are compared with values from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband.
        /// With the method used in this programm, where the pressure loss per length has a linear
        /// relationship with the length, it is obvious that we have to accept higher error tolerance
        /// at higher lengths. A safety pressure of 2.0 bar is recommended for the programm, therefore we test smaller length values against
        /// 1.0 bar safety pressure and higher length values against 2.0 bar safety pressure.
        /// </summary>
        [TestMethod]
        public void PressureLossFrictionPerDistanceTest1()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();
            var values = this.CreateTestValues();

            foreach (var value in values)
            {
                var pressureLossCalc = hydCalc.PressureLossFrictionPerDistance(value.PipeDiameter, value.FlowRate) * value.Length;
                if (value.Length > 1000)
                {
                    Assert.AreEqual(value.PressureLoss, pressureLossCalc, 2.0);
                }
                else
                {
                    Assert.AreEqual(value.PressureLoss, pressureLossCalc, 1.0);
                }
            }
        }

        /// <summary>
        /// Formulas from https://www.schweizer-fn.de/stroemung/druckverlust/druckverlust.php#druckverlustrohr
        /// are compared with values from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband.
        /// The values give the linear decrease of pressure per 100m. The values in the test-set have an
        /// accuracy of 0.2bar / 100m.
        /// </summary>
        [TestMethod]
        public void PressureLossFrictionPerDistanceTest2()
        {
            HydrodynamicCalculator hydCalc = new HydrodynamicCalculator();
            var values = this.CreateTestValues2();

            foreach (var value in values)
            {
                var pressureLossCalc = hydCalc.PressureLossFrictionPerDistance(value.PipeDiameter, value.FlowRate) * value.Length;
                Assert.AreEqual(value.PressureLoss, pressureLossCalc, 0.2);
            }
        }

        /// <summary>
        /// Test against table of measured pressure losses of a specific hose, at a specific flow rate
        /// measured after a specific length.
        /// Values from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband.
        /// </summary>
        private List<PressureLossTestValues> CreateTestValues()
        {
            var values = new List<PressureLossTestValues>();
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 1500,
                    Length = 3000,
                    PressureLoss = 2.55,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 1500,
                    Length = 3000,
                    PressureLoss = 2.55,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 2000,
                    Length = 2000,
                    PressureLoss = 3.10,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 3750,
                    Length = 1500,
                    PressureLoss = 6.98,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 3250,
                    Length = 2000,
                    PressureLoss = 9.20,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 1100,
                    Length = 150,
                    PressureLoss = 0.44,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 1300,
                    Length = 900,
                    PressureLoss = 3.42,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 1800,
                    Length = 1000,
                    PressureLoss = 6.70,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 2400,
                    Length = 600,
                    PressureLoss = 6.54,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 2700,
                    Length = 300,
                    PressureLoss = 4.02,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 55,
                    FlowRate = 350,
                    Length = 100,
                    PressureLoss = 1.10,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 55,
                    FlowRate = 800,
                    Length = 50,
                    PressureLoss = 2.53,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 250,
                    Length = 800,
                    PressureLoss = 0.88,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 450,
                    Length = 500,
                    PressureLoss = 1.75,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 800,
                    Length = 200,
                    PressureLoss = 1.96,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 1800,
                    Length = 150,
                    PressureLoss = 6.38,
                });

            return values;
        }

        /// <summary>
        /// Test against graph with messured pressure losses per 100m hose.
        /// Values were read out manually from the graph with an estimated accuracy of 0.2 bar / 100m.
        /// Values from "Grundschule im Feuerwehrdienst", Chapter 10,
        /// published in 1996 (re-published in 2002/2006), by Schweizerischer Feuerwehrverband.
        /// </summary>
        private List<PressureLossTestValues> CreateTestValues2()
        {
            var values = new List<PressureLossTestValues>();
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 350,
                    Length = 100,
                    PressureLoss = 0.2,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 550,
                    Length = 100,
                    PressureLoss = 0.5,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 688,
                    Length = 100,
                    PressureLoss = 0.75,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 75,
                    FlowRate = 888,
                    Length = 100,
                    PressureLoss = 1.25,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 913,
                    Length = 100,
                    PressureLoss = 0.2,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 1413,
                    Length = 100,
                    PressureLoss = 0.45,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 1913,
                    Length = 100,
                    PressureLoss = 0.775,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 2525,
                    Length = 100,
                    PressureLoss = 1.25,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 110,
                    FlowRate = 3025,
                    Length = 100,
                    PressureLoss = 1.675,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 1250,
                    Length = 100,
                    PressureLoss = 0.05,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 1750,
                    Length = 100,
                    PressureLoss = 0.1,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 2750,
                    Length = 100,
                    PressureLoss = 0.25,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 3750,
                    Length = 100,
                    PressureLoss = 0.45,
                });
            values.Add(
                new PressureLossTestValues()
                {
                    PipeDiameter = 150,
                    FlowRate = 4500,
                    Length = 100,
                    PressureLoss = 0.6,
                });

            return values;
        }
    }
}
