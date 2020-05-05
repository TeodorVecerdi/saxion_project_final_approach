using System;
using System.Globalization;

namespace GXPEngine {
    /// <summary>
    ///     Representation of 2D vectors and points.
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>, IFormattable {
        /// <summary>
        ///     X component of the vector
        /// </summary>
        public float x;

        /// <summary>
        ///     Y component of the vector
        /// </summary>
        public float y;

        /// <summary>
        ///     Z component of the vector
        /// </summary>
        public float z;

        public Vector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(float x, float y) {
            this.x = x;
            this.y = y;
            z = 0F;
        }

        public void Set(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Scale(Vector3 scale) {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public void Normalize() {
            var mag = magnitude;
            if (mag > float.Epsilon)
                this = this / mag;
            else
                this = zero;
        }

        public Vector2 normalized {
            get {
                var v = new Vector2(x, y);
                v.Normalize();
                return v;
            }
        }

        public override bool Equals(object other) {
            if (!(other is Vector3)) return false;

            return Equals((Vector3) other);
        }

        public bool Equals(Vector3 other) {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        /// <summary>
        ///     Returns a nicely formatted string for the vector
        /// </summary>
        public override string ToString() {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            return string.Format("({0}, {1}, {2})", x.ToString(format, formatProvider), y.ToString(format, formatProvider),
                z.ToString(format, formatProvider));
        }

        /// <summary>
        ///     Returns the length of this vector
        /// </summary>
        public float magnitude => Mathf.Sqrt(x * x + y * y + z * z);

        /// <summary>
        ///     Returns the squared length of this vector
        /// </summary>
        public float sqrMagnitude => x * x + y * y + z * z;

        public static Vector3 operator +(Vector3 a, Vector3 b) {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b) {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b) {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b) {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 operator -(Vector3 a) {
            return new Vector3(-a.x, -a.y, -a.z);
        }

        public static Vector3 operator *(Vector3 a, float d) {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator *(float d, Vector3 a) {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator /(Vector3 a, float d) {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs) {
            var diffX = lhs.x - rhs.x;
            var diffY = lhs.y - rhs.y;
            var diffZ = lhs.z - rhs.z;
            return diffX * diffX + diffY * diffY + diffZ * diffZ < Mathf.Epsilon;
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs) {
            return !(lhs == rhs);
        }

        public static Vector3 zero { get; } = new Vector3(0F, 0F, 0F);
        public static Vector3 one { get; } = new Vector3(1F, 1F, 1F);
        public static Vector3 up { get; } = new Vector3(0F, 1F, 0F);
        public static Vector3 down { get; } = new Vector3(0F, -1F, 0F);
        public static Vector3 left { get; } = new Vector3(-1F, 0F, 0F);
        public static Vector3 right { get; } = new Vector3(1F, 0F, 0F);
        public static Vector3 forward { get; } = new Vector3(0F, 0F, 1F);
        public static Vector3 back { get; } = new Vector3(0F, 0F, -1F);
        public static Vector3 positiveInfinity { get; } =
            new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        public static Vector3 negativeInfinity { get; } =
            new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    }
}