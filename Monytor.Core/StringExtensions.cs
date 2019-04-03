using System;

namespace Monytor.Core {
    public static class StringExtensions {
        public static bool EqualsIgnoreCase(this string value, string compareValue, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase) {
            return string.Equals(value, compareValue, stringComparison);
        }
    }
}