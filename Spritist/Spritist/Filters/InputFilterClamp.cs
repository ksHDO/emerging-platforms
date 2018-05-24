using System;
using Android.Text;
using Java.Lang;
using Math = System.Math;

namespace Spritist.Filters
{
    public class InputFilterClamp : Java.Lang.Object, IInputFilter
    {
        private int min = 0;
        private int max = 0;

        public InputFilterClamp(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest,
                                             int dstart, int dend)
        {
            string val = dest.ToString().Insert(dstart, source.ToString());
            if (!int.TryParse(val, out int input))
            {
                throw new System.Exception("Value is not a valid number");
            }

            if (InRange(input, min, max))
            {
                return null;
            }
            
            return new Java.Lang.String(string.Empty);
        }

        private bool InRange(int input, int min, int max)
        {
            if (min < max)
            {
                return input >= min && input <= max;
            }

            return false;
        }
    }
}