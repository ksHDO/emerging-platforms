using System;
using Android.Graphics;
using Android.Media;

namespace Spritist.Utilities
{
    public static class MathUtils
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable
        {
            if (min.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException($"[{nameof(min)}: {min}] is greater than [{nameof(max)}: {max}");

            T output = val;
            if (val.CompareTo(min) < 0)
                output = min;
            if (val.CompareTo(max) > 0)
                output = max;
            return output;
        }

        /// <summary>
        /// Returns in the specified value is within the min and max values, inclusive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool InRange<T>(T val, T min, T max) where T : IComparable
        {
            return (val.CompareTo(min) >= 0 && val.CompareTo(max) <= 0);
        }

        /// <summary>
        /// Blends a color together.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="srcA">The source a.</param>
        /// <param name="destA">The dest a.</param>
        /// <param name="newAlpha">The new alpha.</param>
        /// <returns></returns>
        private static byte BlendColor(byte src, byte dest, float srcA, float destA, float newAlpha)
        {
            return (byte) (((src * srcA) + ((dest * destA) * (1 - srcA))) / newAlpha);
        }

        /// <summary>
        /// Blends two colors together. The destination is what's being drawn ontop of.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Color BlendColors(Color destination, Color source)
        {
            float oldColorA  = destination.A / 255f;
            float drawColorA = source.A / 255f;

            float newAlpha = drawColorA + oldColorA * (1 - drawColorA);

            return new Color(
                BlendColor(source.R, destination.R, drawColorA, oldColorA, newAlpha),
                BlendColor(source.G, destination.G, drawColorA, oldColorA, newAlpha),
                BlendColor(source.B, destination.B, drawColorA, oldColorA, newAlpha),
                (byte) (newAlpha * 255)
            );
        }
    }
}