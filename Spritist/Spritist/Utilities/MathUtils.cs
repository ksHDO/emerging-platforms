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
    }
}