// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    /// <summary>
    /// This class represents single element (coefficient, e.g. c0 and a corresponding exponent) of a
    /// poylinomial in form of y(x) = c0 + c1 * x + c2 * x^2 + c3 * x^3 + ...
    /// </summary>
    public class PolynomElement
    {
        /// <summary>
        /// Gets or sets polynom coefficient.
        /// </summary>
        public double PolynomCoefficient { get; set; }

        /// <summary>
        /// Gets or sets exponent corresponding to PolynomCoefficient.
        /// </summary>
        public int Exponent { get; set; }
    }
}
