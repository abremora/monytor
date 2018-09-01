using System;

namespace Monytor.Infrastructure.Helper {
    public class UtcNowMarker : MarkerBase {
        public override string Tag => "DATETIME.UTCNOW";

        public override Func<string, string> PlaceholderResult
            => (string value) => DateTime.UtcNow.ToString("o");
    }
}
