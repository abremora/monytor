using System;

namespace Monytor.Infrastructure.Helper {
    public abstract class MarkerBase {
        public string StartingPlaceholder { get; set; } = "{{";
        public string ClosingPlaceholder { get; set; } = "}}";

        public abstract string Tag { get; }
        public abstract Func<string, string> PlaceholderResult { get; }


        public string ReplaceMarker(string text) {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var start = $"{StartingPlaceholder}{Tag}";
            if (text.Contains(start)) {
                var textBetween = GetTextBetweenTags(text, start, ClosingPlaceholder);

                text = AddValue(PlaceholderResult(textBetween), start, text);
                text = RemoveTags(text, start);
            }

            return text;
        }

        private static string AddValue(string value, string prefix, string text) {
            var startIndex = text.IndexOf(prefix);
            return text.Insert(startIndex, value);
        }

        private string RemoveTags(string text, string start) {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var startIndex = text.IndexOf(start);
            if (startIndex < 0) return text;

            var endIndex = text.IndexOf(ClosingPlaceholder, startIndex);
            if (endIndex < 0) return text;

            return text.Remove(startIndex, endIndex - startIndex + ClosingPlaceholder.Length);
        }

        private static string GetTextBetweenTags(string text, string startTag, string endTag) {
            var result = text?.Split(new[] { startTag }, StringSplitOptions.None);
            if (result.Length == 1) return string.Empty;

            return result[1].Split(new[] { endTag }, StringSplitOptions.None)[0];
        }
    }
}