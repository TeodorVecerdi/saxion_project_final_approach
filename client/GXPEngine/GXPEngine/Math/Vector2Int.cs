using System;
using System.Globalization;

namespace GXPEngine {
    /// <summary>
    ///     Representation of 2D vectors and points.
    /// </summary>
    public struct Vector2Int : IEquatable<Vector2Int>, IFormattable {
        /// <summary>
        ///     X component of the vector
        /// </summary>
        public int x;

        /// <summary>
        ///     Y component of the vector
        /// </summary>
        public int y;

        public Vector2Int(int x, int y) {
            this.x = x;
            this.y = y;
        }
        
        public void Set(Vector2Int other) {
            x = other.x;
            y = other.y;
        }
        public void Set(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void Scale(Vector2Int scale) {
            x *= scale.x;
            y *= scale.y;
        }

        public void Normalize() {
            var mag = magnitude;
            if (mag > float.Epsilon)
                this = this / mag;
            else
                this = zero;
        }

        public Vector2Int normalized {
            get {
                var v = new Vector2Int(x, y);
                v.Normalize();
                return v;
            }
        }

        public override bool Equals(object other) {
            if (!(other is Vector2Int)) return false;
            return Equals((Vector2Int) other);
        }

        public bool Equals(Vector2Int other) {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        /// <summary>
        ///     Returns a nicely formatted string for the vector
        /// </summary>
        public override string ToString() {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format))
                format = "D";
            return string.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        /// <summary>
        ///     Returns the length of this vector
        /// </summary>
        public float magnitude => Mathf.Sqrt(x * x + y * y);

        /// <summary>
        ///     Returns the squared length of this vector
        /// </summary>
        public float sqrMagnitude => x * x + y * y;

        /// <summary>
        /// Returns a <see cref="GXPEngine.Vector2Int"/> perpendicular on the <paramref name="inDirection"/> parameter
        /// </summary>
        public static Vector2Int Perpendicular(Vector2Int inDirection) {
            return new Vector2Int(-inDirection.y, inDirection.x);
        }

        /// <summary>
        /// Constructs a Vector2Int from parameters <paramref name="x"/> and <paramref name="y"/>
        /// </summary>
        public static Vector2Int From(int x, int y) {
            return new Vector2Int(x, y);
        }

        public static Vector2Int From(Vector2Int other) {
            return From(other.x, other.y);
        }

        /// <summary>
        ///     Converts a <see cref="GXPEngine.Vector3" /> to a Vector2Int.
        /// </summary>
        public static implicit operator Vector2Int(Vector3 v) {
            return new Vector2Int((int) v.x, (int) v.y);
        }

        /// <summary>
        ///     Converts a Vector2Int to a <see cref="GXPEngine.Vector3" />.
        /// </summary>
        public static implicit operator Vector3(Vector2Int v) {
            return new Vector3(v.x, v.y, 0);
        }
        
        /// <summary>
        ///     Converts a <see cref="GXPEngine.Vector2" /> to a Vector2Int.
        /// </summary>
        public static implicit operator Vector2Int(Vector2 v) {
            return new Vector2Int((int)v.x, (int)v.y);
        }

        /// <summary>
        ///     Converts a Vector2Int to a <see cref="GXPEngine.Vector2" />.
        /// </summary>
        public static implicit operator Vector2(Vector2Int v) {
            return new Vector2(v.x, v.y);
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b) {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator *(Vector2Int a, Vector2Int b) {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        public static Vector2Int operator /(Vector2Int a, Vector2Int b) {
            return new Vector2Int(a.x / b.x, a.y / b.y);
        }

        public static Vector2Int operator -(Vector2Int a) {
            return new Vector2Int(-a.x, -a.y);
        }

        public static Vector2Int operator *(Vector2Int a, float d) {
            return new Vector2Int((int) (a.x * d), (int) (a.y * d));
        }

        public static Vector2Int operator *(float d, Vector2Int a) {
            return new Vector2Int((int) (a.x * d), (int) (a.y * d));
        }

        public static Vector2Int operator /(Vector2Int a, float d) {
            return new Vector2Int((int) (a.x / d), (int) (a.y / d));
        }

        public static bool operator ==(Vector2Int lhs, Vector2Int rhs) {
            var diffX = lhs.x - rhs.x;
            var diffY = lhs.y - rhs.y;
            return diffX * diffX + diffY * diffY < Mathf.Epsilon;
        }

        public static bool operator !=(Vector2Int lhs, Vector2Int rhs) {
            return !(lhs == rhs);
        }

        public static Vector2Int zero { get; } = new Vector2Int(0, 0);
        public static Vector2Int one { get; } = new Vector2Int(1, 1);
        public static Vector2Int up { get; } = new Vector2Int(0, 1);
        public static Vector2Int down { get; } = new Vector2Int(0, -1);
        public static Vector2Int left { get; } = new Vector2Int(-1, 0);
        public static Vector2Int right { get; } = new Vector2Int(1, 0);
        public static Vector2Int positiveInfinity { get; } = new Vector2Int(int.MaxValue, int.MaxValue);
        public static Vector2Int negativeInfinity { get; } = new Vector2Int(int.MinValue, int.MinValue);
    }
}