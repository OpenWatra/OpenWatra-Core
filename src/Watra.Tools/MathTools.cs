// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Tools
{
    /// <summary>
    /// Implementation of some useful math functions.
    /// </summary>
    public static class MathTools
    {
        /// <summary>
        /// Linear interpolation is a method of curve fitting using linear polynomials to construct new data points within the range of a discrete set of known data points.
        /// </summary>
        /// <param name="x">value to interpolate on.</param>
        /// <param name="x0">pin point for linear interpolant x0.</param>
        /// <param name="x1">pin point for linear interpolant x1.</param>
        /// <param name="y0">pin point for linear interpolant y0.</param>
        /// <param name="y1">pin point for linear interpolant y1.</param>
        /// <returns>Returns value y, where y=y0 + ((x - x0) * (y1 - y0) / (x1 - x0)).</returns>
        public static double LinearInterpolation(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                // assume value should be between y0 and y1
                return (y0 + y1) / 2;
            }

            return y0 + ((x - x0) * (y1 - y0) / (x1 - x0));
        }
    }
}
