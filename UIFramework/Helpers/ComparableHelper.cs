using System;
using System.Globalization;

namespace UIFramework.Helpers
{
    internal static class ComparableHelper
    {
        /// <summary>
        /// Safely compares a value to a threshold, handling type mismatches
        /// by converting the threshold to the value's type.
        /// When both values are strings that represent numbers, performs numeric comparison.
        /// </summary>
        public static int SafeCompareTo(object value, IComparable threshold)
        {
            if (!(value is IComparable comparable))
                throw new InvalidOperationException($"Value of type '{value.GetType().Name}' does not implement IComparable");

            // When both are strings, attempt numeric comparison to avoid lexicographic issues
            if (value is string strValue && threshold is string strThreshold)
            {
                if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue) &&
                    double.TryParse(strThreshold, NumberStyles.Any, CultureInfo.InvariantCulture, out double numThreshold))
                {
                    return numValue.CompareTo(numThreshold);
                }

                // Fall back to string comparison if not both numeric
                return string.Compare(strValue, strThreshold, StringComparison.Ordinal);
            }

            var convertedThreshold = (IComparable)Convert.ChangeType(threshold, value.GetType());
            return comparable.CompareTo(convertedThreshold);
        }
    }
}