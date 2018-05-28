using System;

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
    }
}