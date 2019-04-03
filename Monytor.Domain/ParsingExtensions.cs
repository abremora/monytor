using System;
using System.Globalization;

namespace Monytor.Domain {
    public static class ParsingExtensions {
        public static CultureInfo ParsingCulture = CultureInfo.GetCultureInfo("en-US");

        public static DateTimeOffset? TryParseDateTimeOffsetFromString(this string value) {
            if (!string.IsNullOrWhiteSpace(value) && DateTimeOffset.TryParse(value, ParsingCulture, DateTimeStyles.AssumeUniversal, out var offset)) {
                return offset;
            }
            return null;
        }

        public static TimeSpan TryParseTimeSpanFromString(this string value) {
            if (!string.IsNullOrWhiteSpace(value) &&  TimeSpan.TryParse(value, ParsingCulture, out var timeSpan)) {
                return timeSpan;
            }
            return new TimeSpan(0, 0, 0);
        }
    }
}
