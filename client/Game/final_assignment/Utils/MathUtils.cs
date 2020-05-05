using System;
using physics_programming.final_assignment.Components;

namespace physics_programming.final_assignment.Utils {
    public static class MathUtils {
        /// <summary>
        ///     Returns <paramref name="value" /> mapped from one range [<paramref name="minA" />, <paramref name="maxA" />] to
        ///     another range [<paramref name="minB" />, <paramref name="maxB" />]
        /// </summary>
        public static float Map(float value, float minA, float maxA, float minB, float maxB) {
            return (value - minA) / (maxA - minA) * (maxB - minB) + minB;
        }

        /// <summary>
        ///     Returns <paramref name="point" /> projected from one quad (<paramref name="original" />)
        ///     to another quad (<paramref name="target" />)
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="original">The original quad on which <paramref name="point" /> lies on</param>
        /// <param name="target">The target quad on which <paramref name="point" /> will be projected to</param>
        /// <returns><code>Vec2</code> representing the projected point.</returns>
        public static Vec2 MapPointOnQuad(Vec2 point, Quad original, Quad target) {
            var v1 = original.P2 - original.P1;
            var v2 = target.P2 - target.P1;

            var dot = v1.Dot(v2);
            var det = v1.Det(v2);
            var angle = (float) Math.Atan2(det, dot);
            var translatedPoint = point - original.P1;
            var rotatedPoint = translatedPoint;
            rotatedPoint.RotateRadians(angle);
            var translatedRotatedPoint = rotatedPoint + target.P1;
            return translatedRotatedPoint;
        }
    }
}