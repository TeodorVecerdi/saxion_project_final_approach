using System;

namespace GXPEngine {
    /// <summary>
    ///     Contains several functions for doing floating point Math
    /// </summary>
    public static class Mathf {
        /// <summary>
        ///     Constant PI
        /// </summary>
        public const float PI = (float) Math.PI;

        /// <summary>
        ///     A representation of positive infinity
        /// </summary>
        public const float Infinity = float.PositiveInfinity;

        /// <summary>
        ///     A representation of negative infinity
        /// </summary>
        public const float NegativeInfinity = float.NegativeInfinity;

        /// <summary>
        ///     Degrees-to-radians conversion constant
        /// </summary>
        public const float Deg2Rad = PI * 2F / 360F;

        /// <summary>
        ///     Radians-to-degrees conversion constant
        /// </summary>
        public const float Rad2Deg = 1F / Deg2Rad;
        public static volatile float FloatMinNormal = 1.17549435E-38f;
        public static volatile float FloatMinDenormal = float.Epsilon;
        public static bool IsFlushToZeroEnabled = FloatMinDenormal == 0;

        /// <summary>
        ///     Tiny floating point value
        /// </summary>
        public static readonly float Epsilon = IsFlushToZeroEnabled ? FloatMinNormal : FloatMinDenormal;

        /// <summary>
        ///     Returns the absolute value of specified number
        /// </summary>
        public static int Abs(int value) {
            return value < 0 ? -value : value;
        }

        /// <summary>
        ///     Returns the absolute value of specified number
        /// </summary>
        public static float Abs(float value) {
            return value < 0 ? -value : value;
        }

        /// <summary>
        ///     Returns the arc-cosine of the specified number
        /// </summary>
        public static float Acos(float f) {
            return (float) Math.Acos(f);
        }
        
        /// <summary>
        ///     Returns the arc-sine of the specified number
        /// </summary>
        public static float Asin(float f) {
            return (float) Math.Asin(f);
        }

        /// <summary>
        ///     Returns the arctangent of the specified number
        /// </summary>
        public static float Atan(float f) {
            return (float) Math.Atan(f);
        }

        /// <summary>
        ///     Returns the angle whose tangent is the quotent of the specified values
        /// </summary>
        public static float Atan2(float y, float x) {
            return (float) Math.Atan2(y, x);
        }

        /// <summary>
        ///     Returns the smallest integer bigger greater than or equal to the specified number
        /// </summary>
        public static int Ceiling(float a) {
            return (int) Math.Ceiling(a);
        }

        /// <summary>
        ///     Returns the cosine of the specified number in radians
        /// </summary>
        public static float Cos(float f) {
            return (float) Math.Cos(f);
        }

        /// <summary>
        ///     Returns the hyperbolic cosine of the specified number
        /// </summary>
        public static float Cosh(float value) {
            return (float) Math.Cosh(value);
        }

        /// <summary>
        ///     Returns e raised to the given number
        /// </summary>
        public static float Exp(float f) {
            return (float) Math.Exp(f);
        }

        /// <summary>
        ///     Returns the largest integer less than or equal to the specified value
        /// </summary>
        public static int Floor(float f) {
            return (int) Math.Floor(f);
        }

        /// <summary>
        ///     Returns the natural logarithm of the specified number
        /// </summary>
        public static float Log(float f) {
            return (float) Math.Log(f);
        }

        /// <summary>
        ///     Returns the log10 of the specified number
        /// </summary>
        public static float Log10(float f) {
            return (float) Math.Log10(f);
        }

        /// <summary>
        ///     Returns x raised to the power of y
        /// </summary>
        public static float Pow(float x, float y) {
            return (float) Math.Pow(x, y);
        }

        /// <summary>
        /// Returns <paramref name="f"/> rounded to the nearest integer
        /// </summary>
        public static float Round(float f) {
            return (float) Math.Round(f);
        }

        /// <summary>
        /// Returns <paramref name="f"/> rounded to the nearest integer
        /// </summary>
        public static int RoundToInt(float f) {
            return (int) Math.Round(f);
        }

        /// <summary>
        ///     Returns a value indicating the sign of the specified number (-1=negative, 0=zero, 1=positive)
        /// </summary>
        public static int Sign(float f) {
            if (f < 0) return -1;
            if (f > 0) return 1;
            return 0;
        }

        /// <summary>
        ///     Returns a value indicating the sign of the specified number (-1=negative, 0=zero, 1=positive)
        /// </summary>
        public static int Sign(int f) {
            if (f < 0) return -1;
            if (f > 0) return 1;
            return 0;
        }

        /// <summary>
        ///     Returns the sine of the specified number in radians
        /// </summary>
        public static float Sin(float f) {
            return (float) Math.Sin(f);
        }

        /// <summary>
        ///     Returns the hyperbolic sine of the specified number
        /// </summary>
        public static float Sinh(float value) {
            return (float) Math.Sinh(value);
        }

        /// <summary>
        ///     Returns the square root of the specified number
        /// </summary>
        public static float Sqrt(float f) {
            return (float) Math.Sqrt(f);
        }

        /// <summary>
        ///     Returns the tangent of the specified number in radians
        /// </summary>
        public static float Tan(float f) {
            return (float) Math.Tan(f);
        }

        /// <summary>
        ///     Returns the hyperbolic tangent of the specified number
        /// </summary>
        public static float Tanh(float value) {
            return (float) Math.Tanh(value);
        }

        /// <summary>
        ///     Calculates the integral part of the specified number
        /// </summary>
        public static float Truncate(float f) {
            return (float) Math.Truncate(f);
        }

        /// <summary>
        ///     Clamps f in the range [min,max]:
        ///     Returns min if f<min, max if f>max, and f otherwise.
        /// </summary>
        public static float Clamp(float f, float min, float max) {
            return f < min ? min : f > max ? max : f;
        }

        /// <summary>
        ///     Clamps <paramref name="v" /> in the range [<paramref name="min" />, <paramref name="max" />]
        /// </summary>
        /// <param name="v">The value to be clamped</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>
        ///     <paramref name="min" /> if <paramref name="v" /> is less than <paramref name="min" />,
        ///     <paramref name="max" /> if <paramref name="v" /> is greater than <paramref name="max" />,
        ///     and <paramref name="v" /> otherwise.
        /// </returns>
        public static int Clamp(int v, int min, int max) {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        /// <summary>
        ///     Clamps <paramref name="f" /> in the range [0, 1]
        /// </summary>
        /// <param name="f">The value to clamp</param>
        /// <returns>
        ///     0 if <paramref name="f" /> is less than 0,
        ///     1 if <paramref name="f" /> is greater than 1,
        ///     and <paramref name="f" /> otherwise.
        /// </returns>
        public static float Clamp01(float f) {
            if (f < 0f) return 0f;
            if (f > 1f) return 1f;
            return f;
        }

        /// <summary>
        ///     Returns the smallest of the two specified values
        /// </summary>
        public static float Min(float a, float b) {
            return a < b ? a : b;
        }

        /// <summary>
        ///     Returns the smallest of two or more values
        /// </summary>
        public static float Min(params float[] values) {
            var len = values.Length;
            if (len == 0)
                return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
                if (values[i] < m)
                    m = values[i];
            return m;
        }

        /// <summary>
        ///     Returns the smallest of the two specified values
        /// </summary>
        public static int Min(int a, int b) {
            return a < b ? a : b;
        }

        /// <summary>
        ///     Returns the smallest of two or more values
        /// </summary>
        public static int Min(params int[] values) {
            var len = values.Length;
            if (len == 0)
                return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
                if (values[i] < m)
                    m = values[i];
            return m;
        }

        /// <summary>
        ///     Returns the biggest of the two specified values
        /// </summary>
        public static float Max(float a, float b) {
            return a > b ? a : b;
        }

        /// <summary>
        ///     Returns the biggest of two or more values
        /// </summary>
        public static float Max(params float[] values) {
            var len = values.Length;
            if (len == 0)
                return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
                if (values[i] > m)
                    m = values[i];
            return m;
        }

        /// <summary>
        ///     Returns the biggest of the two specified values
        /// </summary>
        public static int Max(int a, int b) {
            return a > b ? a : b;
        }

        /// <summary>
        ///     Returns the biggest of two or more values
        /// </summary>
        public static int Max(params int[] values) {
            var len = values.Length;
            if (len == 0)
                return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
                if (values[i] > m)
                    m = values[i];
            return m;
        }

        /// <summary>
        ///     Interpolates between <paramref name="a" /> and <paramref name="b" />
        ///     by <paramref name="t" />. <paramref name="t" /> is clamped between 0 and 1
        /// </summary>
        public static float Lerp(float a, float b, float t) {
            return a + (b - a) * Clamp01(t);
        }

        /// <summary>
        ///     Interpolates between <paramref name="a" /> and <paramref name="b" />
        ///     by <paramref name="t" /> without clamping the interpolant
        /// </summary>
        public static float LerpUnclamped(float a, float b, float t) {
            return a + (b - a) * t;
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed) {
            float deltaTime = Time.deltaTime;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime) {
            float deltaTime = Time.deltaTime;
            float maxSpeed = Infinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Gradually changes a value towards a desired goal over time.
        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime) {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Mathf.Max(0.0001F, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * deltaTime;
            float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
            float change = current - target;
            float originalTo = target;

            // Clamp maximum speed
            float maxChange = maxSpeed * smoothTime;
            change = Clamp(change, -maxChange, maxChange);
            target = current - change;

            float temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            float output = target + (change + temp) * exp;

            // Prevent overshooting
            if (originalTo - current > 0.0F == output > originalTo) {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }
    }
}