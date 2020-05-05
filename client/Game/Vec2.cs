using System;
using GXPEngine;

namespace physics_programming {
    public struct Vec2 : IEquatable<Vec2> {
        // ReSharper disable once InconsistentNaming
        public float x;

        // ReSharper disable once InconsistentNaming
        public float y;

        public Vec2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     Sets the x and y components of this vector
        /// </summary>
        /// <param name="x">The new x component</param>
        /// <param name="y">The new y component</param>
        public void SetXY(float x, float y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///     Returns the magnitude of this vector
        /// </summary>
        public float magnitude => (float) Math.Sqrt(x * x + y * y);
        /// <summary>
        ///     Returns the magnitude of this vector squared
        /// </summary>
        public float sqrMagnitude => x * x + y * y;

        /// <summary>
        ///     Returns this vector normalized
        /// </summary>
        public Vec2 normalized => magnitude > 0 ? this / magnitude : this;

        /// <summary>
        ///     Returns the dot product between this vector and <paramref name="other" />
        /// </summary>
        /// <param name="other">Other vector</param>
        /// <returns>The dot product between this vector and <paramref name="other" /></returns>
        public float Dot(Vec2 other) {
            return x * other.x + y * other.y;
        }
        
        /// <summary>
        ///     Returns the determinant between this vector and <paramref name="other" />
        /// </summary>
        /// <param name="other">Other vector</param>
        /// <returns>The determinant between this vector and <paramref name="other" /></returns>
        public float Det(Vec2 other) {
            return x * other.y - y * other.x;
        }


        /// <summary>
        ///     Calculates and returns the normal vector of the current vector
        /// </summary>
        /// <returns>Normal of the vector</returns>
        public Vec2 Normal() {
            return new Vec2(-y, x).normalized;
        }

        /// <summary>
        ///     Reflects the current vector by <paramref name="normal" /> using the coefficient of reflection
        ///     <paramref name="bounciness" />
        /// </summary>
        /// <param name="normal">The normal vector</param>
        /// <param name="bounciness">The coefficient of reflection</param>
        public void Reflect(Vec2 normal, float bounciness = 1f) {
            var velocityOut = this - (1 + bounciness) * Dot(normal) * normal;
            x = velocityOut.x;
            y = velocityOut.y;
        }

        /// <summary>
        ///     Normalizes the current vector
        /// </summary>
        public void Normalize() {
            var mag = magnitude;
            if (mag < 0.000001f)
                return;

            x /= mag;
            y /= mag;
        }

        /// <summary>
        ///     Sets the angle of the current vector
        /// </summary>
        /// <param name="degrees">Degrees</param>
        public void SetAngleDegrees(float degrees) {
            SetAngleRadians(Deg2Rad(degrees));
        }

        /// <summary>
        ///     Sets the angle of the current vector
        /// </summary>
        /// <param name="radians">Radians</param>
        public void SetAngleRadians(float radians) {
            var m = magnitude;
            var unit = GetUnitVectorRad(radians);
            SetXY(unit.x * m, unit.y * m);
        }

        /// <summary>
        ///     Returns the angle of the current vector in degrees
        /// </summary>
        /// <returns>The angle of the current vector in degrees</returns>
        public float GetAngleDegrees() {
            return Rad2Deg(GetAngleRadians());
        }

        /// <summary>
        ///     Returns the angle of the current vector in radians
        /// </summary>
        /// <returns>The angle of the current vector in radians</returns>
        public float GetAngleRadians() {
            var n = normalized;
            var angle = Mathf.Atan2(n.y, n.x);
            return angle;
        }

        /// <summary>
        ///     Rotates the current vector by <paramref name="degrees" /> degrees.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        public void RotateDegrees(float degrees) {
            RotateRadians(Deg2Rad(degrees));
        }

        /// <summary>
        ///     Rotates the current vector by <paramref name="radians" /> radians.
        /// </summary>
        /// <param name="radians">Radians</param>
        public void RotateRadians(float radians) {
            var c = Mathf.Cos(radians);
            var s = Mathf.Sin(radians);
            var newX = x * c - y * s;
            var newY = x * s + y * c;
            SetXY(newX, newY);
        }

        /// <summary>
        /// Rotates this Vec2 around <paramref name="point"/> by <paramref name="degrees"/>
        /// </summary>
        /// <param name="point">The point around which to rotate</param>
        /// <param name="degrees">The amount of degrees to rotate by</param>
        public void RotateAroundDegrees(Vec2 point, float degrees) {
            RotateAroundRadians(point, Deg2Rad(degrees));
        }

        /// <summary>
        /// Rotates this Vec2 around <paramref name="point"/> by <paramref name="radians"/>
        /// </summary>
        /// <param name="point">The point around which to rotate</param>
        /// <param name="radians">The amount of radians to rotate by</param>
        public void RotateAroundRadians(Vec2 point, float radians) {
            var copy = this;
            copy -= point;
            copy.RotateRadians(radians);
            copy += point;
            SetXY(copy.x, copy.y);
        }

        
        /// <summary>
        /// Helper function used to convert from degrees to radians 
        /// </summary>
        public static float Deg2Rad(float degrees) {
            return degrees / 180.0f * Mathf.PI;
        }
        
        /// <summary>
        /// Helper function used to convert from radians to degrees 
        /// </summary>
        public static float Rad2Deg(float radians) {
            return radians * 180.0f / Mathf.PI;
        }

        /// <summary>
        /// Returns a unit vector rotated by <paramref name="degrees"/> degrees
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>Unit vector rotated by <paramref name="degrees"/> degrees</returns>
        public static Vec2 GetUnitVectorDeg(float degrees) {
            return GetUnitVectorRad(Deg2Rad(degrees));
        }

        /// <summary>
        /// Returns a unit vector rotated by <paramref name="radians"/> radians
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Unit vector rotated by <paramref name="radians"/> radians</returns>
        public static Vec2 GetUnitVectorRad(float radians) {
            return new Vec2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        /// <summary>
        /// Returns a random unit vector
        /// </summary>
        /// <returns>Random unit vector</returns>
        public static Vec2 RandomUnitVector() {
            var rad = Utils.Random(0, Mathf.PI * 2f);
            return GetUnitVectorRad(rad);
        }

        /// <summary>
        ///     Calculates and returns <paramref name="q" /> projected on the line segment from <paramref name="p0" /> to <paramref name="p1" />
        /// </summary>
        /// <param name="q">Point to project</param>
        /// <param name="p0">Start of line segment</param>
        /// <param name="p1">End of line segment</param>
        /// <returns>
        ///     <paramref name="q" /> projected on the line segment from <paramref name="p0" /> to <paramref name="p1" />
        /// </returns>
        public static Vec2 ProjectPointOnLineSegment(Vec2 q, Vec2 p0, Vec2 p1) {
            var a = p1.x - p0.x;
            var b = p1.y - p0.y;
            var c = p0.y - p1.y;

            var pYN = p0.y * a - p0.x * b - q.x * c - q.y * (c * b / a);
            var pYD = a - c * b / a;
            var pY = pYN / pYD;

            var pXN = q.x * a + q.y * b - pY * b;
            var pXD = a;
            var pX = pXN / pXD;

            return new Vec2(pX, pY);
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) {
            return new Vec2(a.x + b.x, a.y + b.y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b) {
            return new Vec2(a.x - b.x, a.y - b.y);
        }

        public static Vec2 operator *(Vec2 a, float d) {
            return new Vec2(a.x * d, a.y * d);
        }

        public static Vec2 operator *(float d, Vec2 a) {
            return new Vec2(a.x * d, a.y * d);
        }

        public static Vec2 operator /(Vec2 a, float d) {
            if (d < 0.000001f) {
                Console.Error.WriteLine($"Division by zero {a}/{d}. Returning same vector without dividing.");
                return a;
            }

            return new Vec2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vec2 a, Vec2 b) {
            return a.Equals(b);
        }

        public static bool operator !=(Vec2 a, Vec2 b) {
            return !a.Equals(b);
        }

        public static Vec2 operator -(Vec2 a) {
            return new Vec2(-a.x, -a.y);
        }

        public bool Equals(Vec2 other) {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj) {
            return obj is Vec2 other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public override string ToString() {
            return $"({x},{y})";
        }

        public static implicit operator Vec2(Vector2 vector2) {
            return new Vec2(vector2.x, vector2.y);
        }

        public static implicit operator Vec2(Vector2Int vector2Int) {
            return new Vec2(vector2Int.x, vector2Int.y);
        }

        public static implicit operator Vec2(Vector3 vector3) {
            return new Vec2(vector3.x, vector3.y);
        }

        public static implicit operator Vec2((float, float) valueTuple) {
            return new Vec2(valueTuple.Item1, valueTuple.Item2);
        }

        public static readonly Vec2 Zero = new Vec2(0f, 0f);
        public static readonly Vec2 One = new Vec2(1f, 1f);
        public static readonly Vec2 Left = new Vec2(-1f, 0f);
        public static readonly Vec2 Right = new Vec2(1f, 0f);
        public static readonly Vec2 Up = new Vec2(0f, -1f);
        public static readonly Vec2 Down = new Vec2(0f, 1f);
        public static readonly Vec2 PositiveInfinity = new Vec2(float.PositiveInfinity, float.PositiveInfinity);
        public static readonly Vec2 NegativeInfinity = new Vec2(float.NegativeInfinity, float.NegativeInfinity);
    }
}