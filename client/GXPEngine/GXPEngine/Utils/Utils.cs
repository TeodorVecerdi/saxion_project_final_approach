using System;

namespace GXPEngine {
    /// <summary>
    ///     The Utils class contains a number of useful functions.
    /// </summary>
    public static class Utils {
        private static readonly Random random = new Random();

        //------------------------------------------------------------------------------------------------------------------------
        //														CalculateFrameRate()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the current frame rate in frames per second.
        ///     Deprecated use game.fps instead!
        /// </summary>
        public static int frameRate => Game.main.currentFps;

        //------------------------------------------------------------------------------------------------------------------------
        //														Random()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a random value between the specified min (inclusive) and max (exclusive).
        ///     If you want to receive an integer value, use two integers as parameters to this function.
        /// </summary>
        /// <param name='min'>
        ///     Inclusive minimum value: lowest possible random value.
        /// </param>
        /// <param name='max'>
        ///     Exclusive maximum value: the returned value will be smaller than this value.
        /// </param>
        public static int Random(int min, int max) {
            return random.Next(min, max);
        }

        public static float Random(float min, float max) {
            return (float) (random.NextDouble() * (max - min) + min);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														print()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Shows output on the console window.
        ///     Basically, a shortcut for Console.WriteLine() that allows for multiple parameters.
        /// </summary>
        public static void print(params object[] list) {
            for (var i = 0; i < list.Length; i++)
                if (list[i] != null) Console.Write(list[i] + " ");
                else Console.Write("null ");
            Console.WriteLine();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RectsOverlap()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns 'true' if the two specified rectangles overlap or 'false' otherwise.
        /// </summary>
        /// <param name='x1'>
        ///     The x position of the first rectangle.
        /// </param>
        /// <param name='y1'>
        ///     The y position of the first rectangle.
        /// </param>
        /// <param name='width1'>
        ///     The width of the first rectangle.
        /// </param>
        /// <param name='height1'>
        ///     The height of the first rectangle.
        /// </param>
        /// <param name='x2'>
        ///     The x position of the second rectangle.
        /// </param>
        /// <param name='y2'>
        ///     The y position of the second rectangle.
        /// </param>
        /// <param name='width2'>
        ///     The width of the second rectangle.
        /// </param>
        /// <param name='height2'>
        ///     The height of the second rectangle.
        /// </param>
        public static bool RectsOverlap(float x1, float y1, float width1, float height1,
            float x2, float y2, float width2, float height2) {
            if (x1 > x2 + width2) return false;
            if (y1 > y2 + height2) return false;
            if (x2 > x1 + width1) return false;
            if (y2 > y1 + height1) return false;
            return true;
        }

        public static string format(this string format, params object[] args) {
            return string.Format(format, args);
        }
    }
}