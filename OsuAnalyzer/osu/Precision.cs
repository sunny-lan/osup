﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Numerics;

namespace osu.Framework.MathUtils
{
    public static class Precision
    {
        public const float FLOAT_EPSILON = 1e-3f;
        public const double DOUBLE_EPSILON = 1e-7;

        public static bool DefinitelyBigger(float value1, float value2, float acceptableDifference = FLOAT_EPSILON) => value1 - acceptableDifference > value2;

        public static bool DefinitelyBigger(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON) => value1 - acceptableDifference > value2;

        public static bool AlmostBigger(float value1, float value2, float acceptableDifference = FLOAT_EPSILON) => value1 > value2 - acceptableDifference;

        public static bool AlmostBigger(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON) => value1 > value2 - acceptableDifference;

        public static bool AlmostEquals(float value1, float value2, float acceptableDifference = FLOAT_EPSILON) => Math.Abs(value1 - value2) <= acceptableDifference;

        public static bool AlmostEquals(Vector2 value1, Vector2 value2, float acceptableDifference = FLOAT_EPSILON) => AlmostEquals(value1.X, value2.X, acceptableDifference) && AlmostEquals(value1.Y, value2.Y, acceptableDifference);

        public static bool AlmostEquals(double value1, double value2, double acceptableDifference = DOUBLE_EPSILON) => Math.Abs(value1 - value2) <= acceptableDifference;
    }
}