using System.Linq;
using System.Text.RegularExpressions;

namespace Monytor.Infrastructure.Helper {
    public class Interpreter {
        public string StartingPlaceholder { get; set; } = "{{";
        public string ClosingPlaceholder { get; set; } = "}}";
        public int MaxIterations { get; set; } = 50;

        private readonly MarkerBase[] _marker;

        public Interpreter() {
            _marker = new MarkerBase[] {
                 new UtcNowMinusMarker { StartingPlaceholder = StartingPlaceholder, ClosingPlaceholder = ClosingPlaceholder },
                 new UtcNowPlusMarker { StartingPlaceholder = StartingPlaceholder, ClosingPlaceholder = ClosingPlaceholder },
                 new UtcNowMarker { StartingPlaceholder = StartingPlaceholder, ClosingPlaceholder = ClosingPlaceholder }
            }
            .OrderByDescending(x => x.Tag.Length)
            .ToArray();          
        }

        public string ReplacePlaceholder(string textWithPlaceholder) {
            if (string.IsNullOrWhiteSpace(textWithPlaceholder))
                return textWithPlaceholder;

            var currentInteration = 0;
            var regex = new Regex(SurroundByPlaceholder("(.*?)"));
            
            while (currentInteration < MaxIterations && regex.IsMatch(textWithPlaceholder)) {
                foreach(var marker in _marker) {
                    textWithPlaceholder = marker.ReplaceMarker(textWithPlaceholder);
                }

                currentInteration++;
            }

            return textWithPlaceholder;
        }

        private string SurroundByPlaceholder(string text) {
            return $"{StartingPlaceholder}{text}{ClosingPlaceholder}";
        }
    }
}