using System;
using System.Globalization;

namespace Monytor.Domain {
    public static class ParsingExtensions {

        public static DateTimeOffset? TryParseDateTimeOffsetFromString(this string value) {
            if (!string.IsNullOrWhiteSpace(value) && DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var offset)) {
                return offset;
            }
            return null;
        }

        public static TimeSpan TryParseTimeSpanFromString(this string value) {
            if (!string.IsNullOrWhiteSpace(value) &&  TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out var timeSpan)) {
                return timeSpan;
            }
            return new TimeSpan(0, 0, 0);
        }
    }
}
