namespace game.utils {
    public static class MathUtils {
        /// <summary>
        ///     Returns <paramref name="value" /> mapped from one range [<paramref name="minA" />, <paramref name="maxA" />] to
        ///     another range [<paramref name="minB" />, <paramref name="maxB" />]
        /// </summary>
        public static float Map(float value, float minA, float maxA, float minB, float maxB) {
            return (value - minA) / (maxA - minA) * (maxB - minB) + minB;
        }
    }
}