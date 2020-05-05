using System;
using System.Collections.Generic;
using System.Linq;
using GXPEngine;

namespace physics_programming.final_assignment.Utils {
    /// <summary>
    /// Taken from source code of game Rimworld by Ludeon Studios
    /// </summary>
    public static class Rand {
        private static readonly Stack<ulong> stateStack = new Stack<ulong>();
        private static readonly RandomNumberGenerator random = new RandomNumberGenerator();
        private static uint iterations;
        private static readonly List<int> tmpRange = new List<int>();

        static Rand() {
            random.seed = (uint) DateTime.Now.GetHashCode();
        }

        public static int Seed {
            set {
                if (stateStack.Count == 0)
                    Debug.LogError("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.");
                random.seed = (uint) value;
                iterations = 0U;
            }
        }

        public static float Value => random.GetFloat(iterations++);

        public static bool Bool => Value < 0.5;

        public static int Sign => Bool ? 1 : -1;

        public static int Int => random.GetInt(iterations++);

        public static Vector3 UnitVector3 => new Vector3(Gaussian(), Gaussian(), Gaussian()).normalized;

        public static Vector2 UnitVector2 => new Vector2(Gaussian(), Gaussian()).normalized;

        public static Vector2 InsideUnitCircle {
            get {
                Vector2 vector2;
                do {
                    vector2 = new Vector2(Value - 0.5f, Value - 0.5f) * 2f;
                } while (vector2.sqrMagnitude > 1.0);

                return vector2;
            }
        }

        public static Vector3 InsideUnitCircleVec3 {
            get {
                var insideUnitCircle = InsideUnitCircle;
                return new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y);
            }
        }

        private static ulong StateCompressed {
            get => random.seed | ((ulong) iterations << 32);
            set {
                random.seed = (uint) (value & uint.MaxValue);
                iterations = (uint) ((value >> 32) & uint.MaxValue);
            }
        }

        public static void EnsureStateStackEmpty() {
            if (stateStack.Count <= 0)
                return;
            Debug.LogWarning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
            while (stateStack.Any())
                PopState();
        }

        public static float Gaussian(float centerX = 0.0f, float widthFactor = 1f) {
            return Mathf.Sqrt(-2f * Mathf.Log(Value)) * Mathf.Sin(6.283185f * Value) * widthFactor + centerX;
        }

        public static float GaussianAsymmetric(float centerX = 0.0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f) {
            var num = Mathf.Sqrt(-2f * Mathf.Log(Value)) * Mathf.Sin(6.283185f * Value);
            if (num <= 0.0)
                return num * lowerWidthFactor + centerX;
            return num * upperWidthFactor + centerX;
        }

        public static int Range(int min, int max) {
            if (max <= min)
                return min;
            return min + Mathf.Abs(Int % (max - min));
        }

        public static int RangeInclusive(int min, int max) {
            if (max <= min)
                return min;
            return Range(min, max + 1);
        }

        public static float Range(float min, float max) {
            if (max <= (double) min)
                return min;
            return Value * (max - min) + min;
        }

        public static bool Chance(float chance) {
            if (chance <= 0.0)
                return false;
            if (chance >= 1.0)
                return true;
            return Value < (double) chance;
        }

        public static bool ChanceSeeded(float chance, int specialSeed) {
            PushState(specialSeed);
            var flag = Chance(chance);
            PopState();
            return flag;
        }

        public static float ValueSeeded(int specialSeed) {
            PushState(specialSeed);
            var num = Value;
            PopState();
            return num;
        }

        public static float RangeSeeded(float min, float max, int specialSeed) {
            PushState(specialSeed);
            var num = Range(min, max);
            PopState();
            return num;
        }

        public static int RangeSeeded(int min, int max, int specialSeed) {
            PushState(specialSeed);
            var num = Range(min, max);
            PopState();
            return num;
        }

        public static int RangeInclusiveSeeded(int min, int max, int specialSeed) {
            PushState(specialSeed);
            var num = RangeInclusive(min, max);
            PopState();
            return num;
        }

        public static T Element<T>(T a, T b) {
            if (Bool)
                return a;
            return b;
        }

        public static T Element<T>(T a, T b, T c) {
            var num = Value;
            if (num < 0.333330005407333)
                return a;
            if (num < 0.666660010814667)
                return b;
            return c;
        }

        public static T Element<T>(T a, T b, T c, T d) {
            var num = Value;
            if (num < 0.25)
                return a;
            if (num < 0.5)
                return b;
            if (num < 0.75)
                return c;
            return d;
        }

        public static T Element<T>(T a, T b, T c, T d, T e) {
            var num = Value;
            if (num < 0.200000002980232)
                return a;
            if (num < 0.400000005960464)
                return b;
            if (num < 0.600000023841858)
                return c;
            if (num < 0.800000011920929)
                return d;
            return e;
        }

        public static T Element<T>(T a, T b, T c, T d, T e, T f) {
            var num = Value;
            if (num < 0.166659995913506)
                return a;
            if (num < 0.333330005407333)
                return b;
            if (num < 0.5)
                return c;
            if (num < 0.666660010814667)
                return d;
            if (num < 0.833329975605011)
                return e;
            return f;
        }

        public static void PushState() {
            stateStack.Push(StateCompressed);
        }

        public static void PushState(int replacementSeed) {
            PushState();
            Seed = replacementSeed;
        }

        public static void PopState() {
            StateCompressed = stateStack.Pop();
        }

        public static bool MTBEventOccurs(float mtb, float mtbUnit, float checkDuration) {
            if (double.IsPositiveInfinity(mtb))
                return false;
            if (mtb <= 0.0) {
                Debug.LogError("MTBEventOccurs with mtb=" + mtb);
                return true;
            }

            if (mtbUnit <= 0.0) {
                Debug.LogError("MTBEventOccurs with mtbUnit=" + mtbUnit);
                return false;
            }

            if (checkDuration <= 0.0) {
                Debug.LogError("MTBEventOccurs with checkDuration=" + checkDuration);
                return false;
            }

            var num1 = checkDuration / (mtb * (double) mtbUnit);
            if (num1 <= 0.0) {
                Debug.LogError("chancePerCheck is " + num1 + ". mtb=" + mtb + ", mtbUnit=" + mtbUnit + ", checkDuration=" + checkDuration);
                return false;
            }

            var num2 = 1.0;
            if (num1 < 0.0001) {
                while (num1 < 0.0001) {
                    num1 *= 8.0;
                    num2 /= 8.0;
                }

                if (Value > num2)
                    return false;
            }

            return Value < num1;
        }
    }
    
    internal class RandomNumberGenerator {
        private const uint Prime1 = 2654435761;
        private const uint Prime2 = 2246822519;
        private const uint Prime3 = 3266489917;
        private const uint Prime4 = 668265263;
        private const uint Prime5 = 374761393;
        public uint seed = (uint) DateTime.Now.GetHashCode();

        public int GetInt(uint iterations) {
            return (int) GetHash((int) iterations);
        }

        public float GetFloat(uint iterations) {
            return (float) ((GetInt(iterations) - (double) int.MinValue) / uint.MaxValue);
        }

        private uint GetHash(int buffer) {
            var num1 = Rotate(seed + 374761393U + 4U + (uint) (buffer * -1028477379), 17) * 668265263U;
            var num2 = (num1 ^ (num1 >> 15)) * 2246822519U;
            var num3 = (num2 ^ (num2 >> 13)) * 3266489917U;
            return num3 ^ (num3 >> 16);
        }

        private static uint Rotate(uint value, int count) {
            return (value << count) | (value >> (32 - count));
        }
    }
}