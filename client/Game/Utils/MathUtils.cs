using System;

namespace game.utils {
    public static class MathUtils {
        /// <summary>
        ///     Returns <paramref name="value" /> mapped from one range [<paramref name="minA" />, <paramref name="maxA" />] to
        ///     another range [<paramref name="minB" />, <paramref name="maxB" />]
        /// </summary>
        public static float Map(float value, float minA, float maxA, float minB, float maxB) {
            return (value - minA) / (maxA - minA) * (maxB - minB) + minB;
        }

        /// <summary>
        /// Clamps <paramref name="value"/> to be in range [<paramref name="min"/>, <paramref name="max"/>]
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <typeparam name="T">Any type that implements IComparable</typeparam>
        /// <returns><paramref name="value"/> clamped to the range [<paramref name="min"/>, <paramref name="max"/>]</returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable {
            if (value.CompareTo(min) < 0) value = min;
            if (value.CompareTo(max) > 0) value = max;
            return value;
        }
        
        /// <summary>
        /// Constrains <paramref name="value"/> to be in range [<paramref name="min"/>, <paramref name="max"/>]
        /// <para>Alias for <see cref="Clamp{T}"/></para>
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <typeparam name="T">Any type that implements IComparable</typeparam>
        /// <returns><paramref name="value"/> constrained to the range [<paramref name="min"/>, <paramref name="max"/>]</returns>
        public static T Constrain<T>(this T value, T min, T max) where T : IComparable {
            return value.Clamp(min, max);
        }
    }
}