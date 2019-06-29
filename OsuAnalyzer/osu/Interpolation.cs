﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Numerics;

namespace osu.Framework.MathUtils
{
    public static class Interpolation
    {
        public static double Lerp(double start, double final, double amount) => start + (final - start) * amount;

        /// <summary>
        /// Interpolates between 2 values (start and final) using a given base and exponent.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="final">The end value.</param>
        /// <param name="base">The base of the exponential. The valid range is [0, 1], where smaller values mean that the final value is achieved more quickly, and values closer to 1 results in slow convergence to the final value.</param>
        /// <param name="exponent">The exponent of the exponential. An exponent of 0 results in the start values, whereas larger exponents make the result converge to the final value.</param>
        /// <returns></returns>
        public static double Damp(double start, double final, double @base, double exponent)
        {
            if (@base < 0 || @base > 1)
                throw new ArgumentOutOfRangeException($"{nameof(@base)} has to lie in [0,1], but is {@base}.", nameof(@base));
            if (exponent < 0)
                throw new ArgumentOutOfRangeException($"{nameof(exponent)} has to be bigger than 0, but is {exponent}.", nameof(exponent));

            return Lerp(start, final, 1 - Math.Pow(@base, exponent));
        }

        /// <summary>
        /// Interpolates between a set of points using a lagrange polynomial.
        /// </summary>
        /// <param name="points">An array of coordinates. No two x should be the same.</param>
        /// <param name="time">The x coordinate to calculate the y coordinate for.</param>
        public static double Lagrange(ReadOnlySpan<Vector2> points, double time)
        {
            if (points == null || points.Length == 0)
                throw new ArgumentException($"{nameof(points)} must contain at least one point");

            double sum = 0;
            for (int i = 0; i < points.Length; i++)
                sum += points[i].Y * LagrangeBasis(points, i, time);
            return sum;
        }

        /// <summary>
        /// Calculates the Lagrange basis polynomial for a given set of x coordinates. Used as a helper function to compute Lagrange polynomials.
        /// </summary>
        /// <param name="points">An array of coordinates. No two x should be the same.</param>
        /// <param name="base">The index inside the coordinate array which polynomial to compute.</param>
        /// <param name="time">The x coordinate to calculate the basis polynomial for.</param>
        public static double LagrangeBasis(ReadOnlySpan<Vector2> points, int @base, double time)
        {
            double product = 1;
            for (int i = 0; i < points.Length; i++)
                if (i != @base)
                    product *= (time - points[i].X) / (points[@base].X - points[i].X);
            return product;
        }

        /// <summary>
        /// Calculates the Barycentric weights for a Lagrange polynomial for a given set of coordinates. Can be used as a helper function to compute a Lagrange polynomial repeatedly.
        /// </summary>
        /// <param name="points">An array of coordinates. No two x should be the same.</param>
        public static double[] BarycentricWeights(ReadOnlySpan<Vector2> points)
        {
            int n = points.Length;
            double[] w = new double[n];

            for (int i = 0; i < n; i++)
            {
                w[i] = 1;
                for (int j = 0; j < n; j++)
                    if (i != j)
                        w[i] *= points[i].X - points[j].X;
                w[i] = 1.0 / w[i];
            }

            return w;
        }

        /// <summary>
        /// Calculates the Lagrange basis polynomial for a given set of x coordinates based on previously computed barycentric weights.
        /// </summary>
        /// <param name="points">An array of coordinates. No two x should be the same.</param>
        /// <param name="weights">An array of precomputed barycentric weights.</param>
        /// <param name="time">The x coordinate to calculate the basis polynomial for.</param>
        public static double BarycentricLagrange(ReadOnlySpan<Vector2> points, double[] weights, double time)
        {
            if (points == null || points.Length == 0)
                throw new ArgumentException($"{nameof(points)} must contain at least one point");
            if (points.Length != weights.Length)
                throw new ArgumentException($"{nameof(points)} must contain exactly as many items as {nameof(weights)}");

            double numerator = 0;
            double denominator = 0;

            for (int i = 0; i < points.Length; i++)
            {
                // while this is not great with branch prediction, it prevents NaN at control point X coordinates
                if (time == points[i].X)
                    return points[i].Y;

                double li = weights[i] / (time - points[i].X);
                numerator += li * points[i].Y;
                denominator += li;
            }

            return numerator / denominator;
        }
    }
}